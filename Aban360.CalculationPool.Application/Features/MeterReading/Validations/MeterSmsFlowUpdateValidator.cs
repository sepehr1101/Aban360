using Aban360.CalculationPool.Application.Features.Base;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Validations
{
    public class MeterSmsFlowUpdateValidator : BaseValidator<MeterSmsFlowUpdateDto>
    {
        public MeterSmsFlowUpdateValidator()
        {
            RuleFor(m => m.Id)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(m => m.SendDateTime)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
