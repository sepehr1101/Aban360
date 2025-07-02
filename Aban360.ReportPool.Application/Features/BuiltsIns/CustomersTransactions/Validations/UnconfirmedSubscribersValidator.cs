using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Application.Features.Base.Validations;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Validations
{
    public class UnconfirmedSubscribersValidator : BaseValidator<UnconfirmedSubscribersInputDto>
    {
        public UnconfirmedSubscribersValidator()
        {
            RuleFor(customer => customer.ZoneIds)
           .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
           .NotNull().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
