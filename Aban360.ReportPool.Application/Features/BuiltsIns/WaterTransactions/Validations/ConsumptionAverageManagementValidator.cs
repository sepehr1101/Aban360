using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.Base.Validations;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Validations
{
    public class ConsumptionAverageManagementValidator:BaseValidator<ConsumptionAverageManagementInputDto>
    {
        public ConsumptionAverageManagementValidator()
        {
            RuleFor(c => c.FromDateJalali)
                .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(c => c.ToDateJalali)
                .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(c => c.ZoneIds)
                .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(c => c.UsageIds)
                .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
