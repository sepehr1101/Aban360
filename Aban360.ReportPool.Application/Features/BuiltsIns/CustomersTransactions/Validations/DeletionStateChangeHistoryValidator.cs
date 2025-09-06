using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.Base.Validations;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Validations
{
    public class DeletionStateChangeHistoryValidator : BaseValidator<DeletionStateChangeHistoryInputDto>
    {
        public DeletionStateChangeHistoryValidator()
        {
            RuleFor(customer => customer.FromDeletionStateIds)
            .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
            .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(customer => customer.ToDeletionStateIds)
            .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
            .NotNull().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
