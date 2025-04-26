using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using DNTPersianUtils.Core;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Bill.Validations
{
    public class TariffCalculationValidator : BaseValidator<TariffTestInput>
    {
        public TariffCalculationValidator()
        {
            RuleFor(i => i.BillId)
              .NotNull().WithMessage(ExceptionLiterals.NotNUll)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(i => i.PreviousReadingDate)
                .LessThan(DateTime.Now.ToShortPersianDateTimeString())
                .WithMessage(ExceptionLiterals.CurrentDateNotMoreThanPreviousDate)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(i => i.PreviousReadingNumber)
                .LessThan(i => i.CurrentReadingNumber)
                .WithMessage(ExceptionLiterals.CurrentNumberNotMoreThanPreviousNumber)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);
            
            RuleFor(i => i.CurrentReadingNumber)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);


        }
    }
}
