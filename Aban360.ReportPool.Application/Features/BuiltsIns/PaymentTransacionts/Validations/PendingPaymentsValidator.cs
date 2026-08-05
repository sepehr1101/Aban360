using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.Base.Validations;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Validations
{
    public class PendingPaymentsValidator : BaseValidator<PendingPaymentsInputDto>
    {
        public PendingPaymentsValidator()
        {
            RuleFor(payment => payment)
                .Must(input => DateValidate(input.FromDateJalali,input.ToDateJalali).IsValid).WithMessage(input => DateValidate(input.FromDateJalali, input.ToDateJalali).ErrorMessage);

            RuleFor(payment => payment.ZoneIds)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(input => input)
                 .Must(input => FromToDateJalaliValidation.DateValidation(new FromToDateJalaliDto(input.FromDateJalali,
                                                                                                 input.ToDateJalali)).IsValid)
                 .WithMessage(input => FromToDateJalaliValidation.DateValidation(new FromToDateJalaliDto(input.FromDateJalali,
                                                                                                 input.ToDateJalali)).ErrorMessage);

        }
    }
}