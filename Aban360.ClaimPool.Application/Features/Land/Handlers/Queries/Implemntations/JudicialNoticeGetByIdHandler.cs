using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Base;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class JudicialNoticeGetByIdHandler : IJudicialNoticeGetByIdHandler
    {
        private readonly IConnectDisconnectQueryService _connectDisconnectQueryService;
        private readonly IConCompanyQueryService _conCompanyQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly ICommonZoneService _commonZoneService;
        static string _title = ReportLiterals.JudicalNoticeCommand;
        public JudicialNoticeGetByIdHandler(
            IConnectDisconnectQueryService connectDisconnectQueryService,
            IConCompanyQueryService conCompanyQueryService,
            ICommonMemberQueryService commonMemberQueryService,
            ICommonZoneService commonZoneService)
        {
            _connectDisconnectQueryService = connectDisconnectQueryService;
            _connectDisconnectQueryService.NotNull(nameof(connectDisconnectQueryService));

            _conCompanyQueryService = conCompanyQueryService;
            _conCompanyQueryService.NotNull(nameof(conCompanyQueryService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));
        }

        public async Task<FlatReportOutput<JudicalNoticeCommandHeaderOutputDto, JudicalNoticeCommandDataOutputDto>> Handle(long id, IAppUser appUser, CancellationToken cancellationToken)
        {
            ConnectDisconnectGetDto connectDisconnectResult = await _connectDisconnectQueryService.Get(id, ReportLiterals.JudicalNoticeId);
            ConCompanyGetDto conCompanyInfo = await _conCompanyQueryService.GetValid(connectDisconnectResult.CompanyId ?? 0);
            ZoneIdAndCustomerNumber zoneIdAndCustomeorNumber = await _commonMemberQueryService.Get(connectDisconnectResult.BillId);
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(zoneIdAndCustomeorNumber);
            await _commonZoneService.IsUserInZone(appUser, zoneIdAndCustomeorNumber.ZoneId);

            return await GetResult(conCompanyInfo, memberInfo, cancellationToken);
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
        private string GetInstedOfEmpty(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? "-" : value;
        }
    }
}
