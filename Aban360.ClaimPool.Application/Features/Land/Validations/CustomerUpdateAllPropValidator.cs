using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Validations
{
    public class CustomerUpdateAllPropValidator : BaseValidator<CustomerUpdateInputDto>
    {
        public CustomerUpdateAllPropValidator()
        {
            RuleFor(f => f.Id)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.ZoneId)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.CustomerNumber)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.BillId)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.MobileNumber)
                .Must(IsValidMobileNumberOrNull).WithMessage(ExceptionLiterals.MobileNumberFormat);

            RuleFor(f => f.NationalCode)
                .Must(IsValidNullableNationalCode).WithMessage(ExceptionLiterals.NationalCodeFormat);

            RuleFor(f => f.PostalCode)
                .Must(IsValidNullablePostalCode).WithMessage(ExceptionLiterals.PostalCodeFormat);
        }
    }
}