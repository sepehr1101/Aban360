using Aban360.Common.Literals;
using DNTPersianUtils.Core;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.Base.Validations
{
    public class BaseValidator<T> : AbstractValidator<T>
    {
        public BaseValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
        }
        protected virtual (bool IsValid, string ErrorMessage) DateValidate(string FromDateJalali, string ToDateJalali)
        {
            if (FromDateJalali.CompareTo(ExceptionLiterals.WaterBillMinDate) < 0)
                return (false, ExceptionLiterals.FromDateMoreThanDate(ExceptionLiterals.WaterBillMinDate));

            if (!string.IsNullOrWhiteSpace(FromDateJalali) && FromDateJalali.Length != 10)
                return (false, ExceptionLiterals.Equal10);

            if (ToDateJalali.CompareTo((DateTime.Now.AddDays(-1).ToShortPersianDateString())) > 0)
                return (false, ExceptionLiterals.ToDateLessThanDate(DateTime.Now.ToShortPersianDateString()));

            if (!string.IsNullOrWhiteSpace(ToDateJalali) && ToDateJalali.Length != 10)
                return (false, ExceptionLiterals.Equal10);

            return (true, "");
        }
    }
}
