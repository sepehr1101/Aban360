using Aban360.CalculationPool.Application.Features.Base;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Validations
{
    public class ChangeDateBatchInputValidator : BaseValidator<ChangeDateBatchInputDto>
    {
        public ChangeDateBatchInputValidator()
        {
            RuleFor(t => t.MeterFlowId)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull);
            
            RuleFor(t => t.FromReadingNumber)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull);
            
            RuleFor(t => t.ToReadingNumber)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull);
            
            RuleFor(t => t.DateJalali)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

        }
    }
}
