using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.Base.Validations;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using DNTPersianUtils.Core;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Validations
{
    public class PendingPaymentsValidator : BaseValidator<PendingPaymentsInputDto>
    {
        public PendingPaymentsValidator()
        {
            RuleFor(payment => payment)
                .Must(input=>ValidationDate(input).IsValid).WithMessage(input=>ValidationDate(input).ErrorMessage);

            RuleFor(payment => payment.ZoneIds)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(input=>input)
                 .Must(input=> FromToDateJalaliValidation.DateValidation(new FromToDateJalaliDto(input.FromDateJalali,
                                                                                                 input.ToDateJalali)).IsValid)
                 .WithMessage(input=> FromToDateJalaliValidation.DateValidation(new FromToDateJalaliDto(input.FromDateJalali,
                                                                                                 input.ToDateJalali)).ErrorMessage);

        }
       
        private (bool IsValid,string ErrorMessage) ValidationDate(PendingPaymentsInputDto input)
        {
            if (input.FromDateJalali.CompareTo(ExceptionLiterals.WaterBillMinDate) < 0)
                return (false, ExceptionLiterals.FromDateMoreThanDate(ExceptionLiterals.WaterBillMinDate));

            if (!string.IsNullOrWhiteSpace(input.FromDateJalali) && input.FromDateJalali.Length != 10)
                return (false, ExceptionLiterals.Equal10);

            if (input.ToDateJalali.CompareTo((DateTime.Now.AddDays(-1).ToShortPersianDateString())) > 0)
                return (false, ExceptionLiterals.ToDateLessThanDate(DateTime.Now.ToShortPersianDateString()));

            if (!string.IsNullOrWhiteSpace(input.ToDateJalali) && input.ToDateJalali.Length != 10)
                return (false, ExceptionLiterals.Equal10);

            return (true,"");
        }
    }
}