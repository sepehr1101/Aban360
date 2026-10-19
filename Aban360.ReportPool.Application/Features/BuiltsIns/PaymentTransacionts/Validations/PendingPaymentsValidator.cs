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
        //public (bool IsValid, string ErrorMessage) DateValidate(string FromDateJalali, string ToDateJalali)
        //{
        //    if (FromDateJalali.CompareTo(ExceptionLiterals.WaterBillMinDate) < 0)
        //        return (false, ExceptionLiterals.FromDateMoreThanDate(ExceptionLiterals.WaterBillMinDate));

        //    if (!string.IsNullOrWhiteSpace(FromDateJalali) && FromDateJalali.Length != 10)
        //        return (false, ExceptionLiterals.Equal10);

        //    if (ToDateJalali.CompareTo((DateTime.Now.AddDays(-1).ToShortPersianDateString())) > 0)
        //        return (false, ExceptionLiterals.ToDateLessThanDate(DateTime.Now.ToShortPersianDateString()));

        //    if (!string.IsNullOrWhiteSpace(ToDateJalali) && ToDateJalali.Length != 10)
        //        return (false, ExceptionLiterals.Equal10);

        //    return (true, "");
        //}
    }
}