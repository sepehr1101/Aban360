using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.Base.Validations;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Validations
{
    public class UnreadValidator:BaseValidator<UnreadInputDto>
    {
        public UnreadValidator()
        {
            RuleFor(customer => customer.FromReadingNumber)
          .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
          .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(customer => customer.ToReadingNumber)
           .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
           .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(customer => customer.PeriodCount)
           .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
           .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(customer => customer.ZoneIds)
           .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
           .NotNull().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
