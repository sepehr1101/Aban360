using Aban360.CalculationPool.Application.Features.Base;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Validations
{
    public class MeterReadingFileCreateValidator:BaseValidator<MeterReadingFileCreateDto>
    {
        public MeterReadingFileCreateValidator()
        {
            RuleFor(t => t.ReadingFile)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

        }
    }
}
