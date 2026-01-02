using Aban360.CalculationPool.Application.Features.Base;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Validations
{
    public class ServiceLinkRegisterManualValidator:BaseValidator<ServiceLinkRegisterManualInputDto>
    {
        public ServiceLinkRegisterManualValidator()
        {
            RuleFor(i => i.BankDateJalali)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.BillId)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.BankCode)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.Amount)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.ChequeOrMinutesNumber)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

        }
    }
}
