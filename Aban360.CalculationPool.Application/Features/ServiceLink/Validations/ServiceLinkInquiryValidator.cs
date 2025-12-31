using Aban360.CalculationPool.Application.Features.Base;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Validations
{
    public class ServiceLinkInquiryValidator:BaseValidator<ServiceLinkInquiryInputDto>
    {
        public ServiceLinkInquiryValidator()
        {
            RuleFor(i => i.PaymentId)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.BillId)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
