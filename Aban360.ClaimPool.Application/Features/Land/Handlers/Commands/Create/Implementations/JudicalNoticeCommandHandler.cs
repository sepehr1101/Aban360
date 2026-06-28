using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
{
    internal sealed class JudicalNoticeCommandHandler : AbstractBaseConnection, IJudicalNoticeCommandHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConCompanyQueryService _conCompanyQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly IValidator<JudicalNoticeCommandInputDto> _validator;
        const string _title = "تقاضانامه صدور اجرائیه اسناد ذمه";
        private int _setJudicalTypeId = 2;
        public JudicalNoticeCommandHandler(
            IHttpContextAccessor contextAccessor,
            IConCompanyQueryService conCompanyQueryService,
            ICommonMemberQueryService commonMemberQueryService,
            IValidator<JudicalNoticeCommandInputDto> validator,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _conCompanyQueryService = conCompanyQueryService;
            _conCompanyQueryService.NotNull(nameof(conCompanyQueryService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }
        public async Task<FlatReportOutput<JudicalNoticeCommandHeaderOutputDto, JudicalNoticeCommandDataOutputDto>> Handle(JudicalNoticeCommandInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await Validate(inputDto, cancellationToken);
            ConCompanyGetDto conCompanyInfo = await _conCompanyQueryService.Get(inputDto.CompanyId);
            ZoneIdAndCustomerNumber zoneIdAndCustomeorNumber = await _commonMemberQueryService.Get(inputDto.BillId);
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(zoneIdAndCustomeorNumber);

            return await GetResult(conCompanyInfo, memberInfo, cancellationToken);
        }
        private async Task ExceSql(IAppUser appUser, string opLogText)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    ConnectDisconnectCommandService connectDisconnectCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    //await connectDisconnectCommandService.Insert(connectDisconnectInsertDto);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        //private ConnectDisconnectInsertDto GetConnectDisconnectInsertDto(ReportOutput<CustomerGeneralInfoHeaderDto, CustomerGeneralInfoDataDto> customerInfo, ConnectDisconnectPrintInputDto inputDto, IAppUser appUser, string causeTitle, bool isConnect)
        //{
        //    return new ConnectDisconnectInsertDto()
        //    {
        //        ZoneId = customerInfo.ReportData?.FirstOrDefault()?.ZoneId ?? 0,
        //        ZoneTitle = customerInfo.ReportData?.FirstOrDefault()?.ZoneTitle ?? string.Empty,
        //        BillId = customerInfo.ReportHeader.BillId,
        //        WaterDebt = customerInfo.ReportData?.FirstOrDefault()?.WaterDebtAmount ?? 0,
        //        CommandDateTime = DateTime.Now,
        //        CommandBy = appUser.UserId,
        //        CommandCauseId = inputDto.Why ?? 0,
        //        CommandCauseTitle = causeTitle,
        //        ResultDateTime = null,
        //        ResultBy = null,
        //        ResultId = null,
        //        ResultTitle = null,
        //        MeterDiameterId = customerInfo.ReportHeader.MeterDiameterId,
        //        MeterDiameterTitle = customerInfo.ReportHeader.MeterDiameterTitle,
        //        CompanyTitle = inputDto.Who,
        //        TypeId = isConnect ? _connectTypeId : _disconnectTypeId,
        //        TypeTitle = isConnect ? ReportLiterals.Connect : ReportLiterals.Disconnect,
        //        Description = inputDto.Description ?? string.Empty,
        //    };
        //}
        private async Task Validate(JudicalNoticeCommandInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new BaseException(message);
            }
        }
        private JudicalNoticeCommandDataOutputDto GetData(ConCompanyGetDto conCompanyInfo, MemberInfoGetDto memberInfo)
        {
            return new JudicalNoticeCommandDataOutputDto()
            {
                ZoneTitle = memberInfo.ZoneTitle,
                RegionTitle = memberInfo.RegionTitle,
                CustomerBillId = memberInfo.BillId,
                CustomerNumber = memberInfo.CustomerNumber,
                CustomerReadingNumber = memberInfo.ReadingNumber,
                CustomerFirstName = memberInfo.FirstName,
                CustomerSurname = memberInfo.Surname,
                CustomerFullName = memberInfo.FullName,
                CustomerFatherName = GetInstedOfEmpty(memberInfo.FatherName),
                CustomerAddress = GetInstedOfEmpty(memberInfo.Address),
                CustomerPostalCode = GetInstedOfEmpty(memberInfo.PostalCode),
                CustomerMobileNumber = GetInstedOfEmpty(memberInfo.MobileNumber),
                CustomerNationalCode = GetInstedOfEmpty(memberInfo.NationalCode),
                DebtAmount = memberInfo.DebtAmount ?? 0,
                CustomerCertificateNumber = "-",
                CustomerBirthPlace = "-",
                CustomerBirthDateJalali = "-",
                CompanyName = conCompanyInfo.CompanyName,
                CompanyNationalCode = conCompanyInfo.CompanyNationalCode,
                CompanyMobileNumber = conCompanyInfo.CompanyMobileNumber,
                CompanyCertificateNumber = "-",
                CompanyRegisterPlace = "-",
                CompanyAddress = conCompanyInfo.CompanyAddress,
                CompanyPostalCode = conCompanyInfo.CompanyPostalCode,
                RepresentativeName = conCompanyInfo.RepresentativeName,
                RepresentativeNationalCode = conCompanyInfo.RepresentativeNationalCode,
                RepresentativeFatherName = conCompanyInfo.RepresentativeFatherName,
                RepresentativeMobileNumber = conCompanyInfo.RepresentativeMobileNumber,
                RepresentativeAddress = conCompanyInfo.RepresentativeAddress,
                RepresentativePostalCode = conCompanyInfo.RepresentativePostalCode,
                RepresentativeBirthDateJalali = conCompanyInfo.RepresentativeBirthDateJalali,
                RepresentativeBirthPlace = conCompanyInfo.RepresentativeBirthPlace,
                RepresentativeCertificateNumber = conCompanyInfo.RepresentativeCertificateNumber,
                AdministratorName = conCompanyInfo.AdministratorName,
                AdministratorMobileNumber = conCompanyInfo.AdministratorMobileNumber,
                ContractNumber = conCompanyInfo.ContractNumber,
                ContractDataJalali = conCompanyInfo.ContractDataJalali,
            };
        }
        private async Task<FlatReportOutput<JudicalNoticeCommandHeaderOutputDto, JudicalNoticeCommandDataOutputDto>> GetResult(ConCompanyGetDto conCompanyInfo, MemberInfoGetDto memberInfo, CancellationToken cancellationToken)
        {
            JudicalNoticeCommandHeaderOutputDto header = new()
            {
                ZoneTitle = memberInfo.ZoneTitle,
                RegionTitle = memberInfo.RegionTitle,
                BillId = memberInfo.BillId,
                Title = _title,
                RecordCount = 1,
                Message = string.Format(SmsTemplates.JudicalNoticeCommandAlert, memberInfo.FullName, memberInfo.BillId, memberInfo.DebtAmount, Environment.NewLine),
                JudicalBase64 = await Base64Operation.GetDudicalBase64(cancellationToken),
                JudicalDocumentBase64 = await Base64Operation.GetDudicalDocumentBase64(cancellationToken)
            };
            return new FlatReportOutput<JudicalNoticeCommandHeaderOutputDto, JudicalNoticeCommandDataOutputDto>(_title, header, GetData(conCompanyInfo, memberInfo));
        }
        private string GetInstedOfEmpty(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? "-" : value;
        }
    }
}
