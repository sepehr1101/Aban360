using Aban360.CalculationPool.Application.Features.Base;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Validations
{
    public class MeterReadingExcludedValidator : BaseValidator<MeterReadingDetailExcludedInputDto>
    {
        public MeterReadingExcludedValidator()
        {
            RuleFor(t => t.ZoneId)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(t => t.ExcludeDateJalali)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

        }
    }
}
