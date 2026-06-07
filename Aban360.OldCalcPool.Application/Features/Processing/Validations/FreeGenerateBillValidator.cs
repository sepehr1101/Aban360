using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.Base;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Processing.Validations
{
    public class FreeGenerateBillValidator : BaseValidator<FreeGenerateBillInputDto>
    {
        static int[] _allowedZeroMeterNumberCounterState = { 4, 7 };
        public FreeGenerateBillValidator()
        {
            RuleFor(g => g.BillId)
                .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);
            
            RuleFor(g => g.CurrentDateJalali)
                .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);
            
            RuleFor(g => g.PreviousDateJalali)
                .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(input => input)
                .Must(CheckMeterNumber).WithMessage(ExceptionLiterals.NotNull);

        }
        private bool CheckMeterNumber(FreeGenerateBillInputDto input)
        {
            if (!_allowedZeroMeterNumberCounterState.Contains(input.CounterStateCode ?? 0) && input.CurrentMeterNumber == 0)
            {
                return false;
            }
            return true;
        }
    }
}
