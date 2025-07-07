using Aban360.BlobPool.Application.Features.Base;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Validations
{
    public class ServiceLinkDebtorCustomersValidator : BaseValidator<ServiceLinkDebtorCustomersInputDto>
    {
        public ServiceLinkDebtorCustomersValidator()
        {
            RuleFor(customer => customer.FromAmout)
           .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
           .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(customer => customer.ToAmount)
           .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
           .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(customer => customer.ZoneIds)
           .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
           .NotNull().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
