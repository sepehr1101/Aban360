using Aban360.CalculationPool.Application.Features.Base;
using Aban360.CalculationPool.Domain.Features.CollectBills.Inputs;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Validations
{
    public class MeterReadingDetailByZoneValidator : BaseValidator<MeterFlowByZoneInputDto>
    {
        public MeterReadingDetailByZoneValidator()
        {
            RuleFor(t => t.ZoneId)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(t => t.FromDateJalali)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(t => t.ToDateJalali)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
