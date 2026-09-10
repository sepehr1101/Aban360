using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Validations
{
    public class CustomerHouseholdUpdateValidator : BaseValidator<CustomerHouseholdUpdateInputDto>
    {
        public CustomerHouseholdUpdateValidator()
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
            
            RuleFor(f => f.HouseholdNumber)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.HouseholdDateJalali)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .Must(IsValidNullableDateJalali).WithMessage(ExceptionLiterals.InvalidDate);
        }
    }
}