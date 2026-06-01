using Aban360.CalculationPool.Application.Features.Base;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Validations
{
    public class ServiceLinkReturnValidator : BaseValidator<ServiceLinkReturnInputDto>
    {
        public ServiceLinkReturnValidator()
        {
            RuleFor(f => f.BillId)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.Amount)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
