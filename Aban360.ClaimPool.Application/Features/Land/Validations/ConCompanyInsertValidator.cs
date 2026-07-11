using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Validations
{
    public class ConCompanyInsertValidator : BaseValidator<ConCompanyInsertInputDto>
    {
        public ConCompanyInsertValidator()
        {
            RuleFor(c => c.ZoneId)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(c => c.CompanyName)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(c => c.RepresentativeName)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(c => c.CompanyMobileNumber)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .Must(IsValidMobileNumber).WithMessage(ExceptionLiterals.MobileNumberFormat);

            RuleFor(c => c.RepresentativeMobileNumber)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .Must(IsValidMobileNumber).WithMessage(ExceptionLiterals.MobileNumberFormat);

            RuleFor(c => c.AdministratorName)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(c => c.AdministratorMobileNumber)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .Must(IsValidMobileNumber).WithMessage(ExceptionLiterals.MobileNumberFormat);
        }
    }
}
