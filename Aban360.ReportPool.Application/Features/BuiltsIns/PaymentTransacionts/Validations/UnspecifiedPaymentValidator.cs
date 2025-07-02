using Aban360.BlobPool.Application.Features.Base;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Validations
{
    public class UnspecifiedPaymentValidator : BaseValidator<UnspecifiedPaymentInputDto>
    {
        public UnspecifiedPaymentValidator()
        {
            RuleFor(customer => customer.FromDateJalali)
        .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
        .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(customer => customer.ToDateJalali)
           .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
           .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(customer => customer.FromBankId)
           .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
           .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(customer => customer.ToBankId)
           .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
           .NotNull().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
