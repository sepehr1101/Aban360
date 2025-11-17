using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.Base.Validations;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Validations
{
    public class ClientValidationValidator : BaseValidator<ClientValidationInputDto>
    {
        public ClientValidationValidator()
        {
            RuleFor(customer => customer.ValidationEstate)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
