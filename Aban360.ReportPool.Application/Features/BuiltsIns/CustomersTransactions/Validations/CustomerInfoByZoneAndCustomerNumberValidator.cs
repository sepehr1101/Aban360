using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.Base.Validations;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Validations
{
    public class CustomerInfoByZoneAndCustomerNumberValidator : BaseValidator<CustomerInfoByZoneAndCustomerNumberInputDto>
    {
        public CustomerInfoByZoneAndCustomerNumberValidator()
        {
            RuleFor(customer => customer.ZoneId)
            .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
            .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(customer => customer.CustomerNumber)
            .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
            .NotNull().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
