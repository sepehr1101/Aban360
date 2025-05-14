using Aban360.CalculationPool.Application.Features.Bill.Validations;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using DNTPersianUtils.Core;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.TestTariff.Validations
{
    public class TariffCalculationValidator : BaseValidator<TariffTestInput>
    {
        public TariffCalculationValidator()
        {
            RuleFor(i => i.BillId)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.PreviousReadingDate)
                .LessThan(i => i.CurrentReadingDate)
                .Must(previousReadingDate => previousReadingDate.ToPersianDateTime() is not null && previousReadingDate.ToPersianDateTime().IsValidDateTime)
                .WithMessage(ExceptionLiterals.PreviousDateIsInvalid)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.CurrentReadingDate)
                .Must(currentReadingDate => currentReadingDate.ToPersianDateTime() is not null && currentReadingDate.ToPersianDateTime().IsValidDateTime)
                .LessThan(DateTime.Now.ToShortPersianDateTimeString())
                .WithMessage(ExceptionLiterals.CurrentDateIsInvalid)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.PreviousReadingNumber)
                .LessThan(i => i.CurrentReadingNumber)
                .WithMessage(ExceptionLiterals.CurrentNumberNotMoreThanPreviousNumber)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.CurrentReadingNumber)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
