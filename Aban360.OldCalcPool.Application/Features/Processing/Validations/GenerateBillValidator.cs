using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.Base;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Processing.Validations
{
    public class GenerateBillValidator : BaseValidator<GenerateBillInputDto>
    {
        static int[] _allowedZeroMeterNumberCounterState = { 4, 7 };
        public GenerateBillValidator()
        {
            RuleFor(g => g.BillId)
                .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(input => input)
                .Must(CheckMeterNumber).WithMessage(ExceptionLiterals.NotNull);

        }
        private bool CheckMeterNumber(GenerateBillInputDto input)
        {
            if (!_allowedZeroMeterNumberCounterState.Contains(input.CounterStateCode ?? 0) && input.MeterNumber == 0)
            {
                return false;   
            }
            return true;
        }
    }
}
