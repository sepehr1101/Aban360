using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.Base.Validations;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Validations
{
    public class UseStateChangeHistoryValidator : BaseValidator<UseStateChangeHistoryInputDto>
    {
        public UseStateChangeHistoryValidator()
        {
            RuleFor(customer => customer.FromUseStateIds)
            .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
            .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(customer => customer.ToUseStateIds)
            .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
            .NotNull().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
