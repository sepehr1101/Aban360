using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.Base.Validations;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Validations
{
    public class ReadingChecklistValidator : BaseValidator<ReadingChecklistInputDto>
    {
        public ReadingChecklistValidator()
        {
            RuleFor(meter => meter.FromReadingNumber)
               .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
               .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(meter => meter.ToReadingNumber)
                .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(meter => meter.ZoneId)
                .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
