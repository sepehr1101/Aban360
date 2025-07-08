using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Validations
{
    public class HouseholdRegisterUpdateValidator : BaseValidator<HouseholdRegisterUpdateDto>
    {
        public HouseholdRegisterUpdateValidator()
        {
            RuleFor(f => f.BillId)
                 .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                 .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.HouseholdDateJalali)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.HouseholdNumber)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
