using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Application.Features.Base.Validations;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Validations
{
    public class CustomerSearchValidator : BaseValidator<CustomerSearchInputDto>
    {
        public CustomerSearchValidator()
        {

            RuleFor(customer => customer.InputText)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
