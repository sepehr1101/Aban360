using Aban360.CalculationPool.Application.Features.Bill.Validations;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Sale.Validations
{
    public class InstallationAndEquipmentUpdateValidator : BaseValidator<InstallationAndEquipmentUpdateDto>
    {
        public InstallationAndEquipmentUpdateValidator()
        {
            RuleFor(i => i.Id)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.EquipmentAmount)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.MeterDiameterId)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.InstallationAmount)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.FromDateJalali)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.ToDateJalali)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

        }
    }
}
