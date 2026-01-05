using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.Base.Validations;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Validations
{
    public class CustomerMiniSearchValidator : BaseValidator<CustomerMiniSearchInputDto>
    {
        public CustomerMiniSearchValidator()
        {
            RuleFor(customer => customer.Input)
            .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
            .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(customer => customer.SearchType)
            .IsInEnum().WithMessage(ExceptionLiterals.MustEnum)
            .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
            .NotNull().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
