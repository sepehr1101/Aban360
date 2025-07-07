using Aban360.BlobPool.Application.Features.Base;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Validations
{
    public class ServiceLinkCalculationDetailsValidator : BaseValidator<ServiceLinkCalculationDetailsInputDto>
    {
        public ServiceLinkCalculationDetailsValidator()
        {
            RuleFor(calc => calc.Input)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
