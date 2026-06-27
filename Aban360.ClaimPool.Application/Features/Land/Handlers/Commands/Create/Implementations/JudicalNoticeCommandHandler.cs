using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
{
    internal sealed class JudicalNoticeCommandHandler : IJudicalNoticeCommandHandler
    {
        private readonly IConCompanyQueryService _conCompanyQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly IValidator<JudicalNoticeCommandInputDto> _validator;
        const string _title = "تقاضانامه صدور اجرائیه اسناد ذمه";
        public JudicalNoticeCommandHandler(
            IConCompanyQueryService conCompanyQueryService,
            ICommonMemberQueryService commonMemberQueryService,
            IValidator<JudicalNoticeCommandInputDto> validator)
        {
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
                CustomerFatherName = memberInfo.FatherName,
                CustomerAddress = memberInfo.Address,
                CustomerPostalCode = memberInfo.PostalCode,
                CustomerMobileNumber = memberInfo.MobileNumber,
                CustomerNationalCode = memberInfo.NationalCode,
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
    }
}
