using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.Base.Validations;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Validations
{
    public class LowWorkingMeterValidator : BaseValidator<ConsumptionAverageAnalysisInputDto>
    {
        public LowWorkingMeterValidator()
        {
            RuleFor(l => l.Values)
              .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(l => l.FromDateJalali)
              .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(l => l.ToDateJalali)
                .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(l => l.ZoneIds)
               .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
               .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(l => l.UsageIds)
                .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
