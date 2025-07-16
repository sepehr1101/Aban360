using Aban360.Common.Literals;
using DNTPersianUtils.Core;

namespace Aban360.ReportPool.Application.Features.Base.Validations
{
    public static class FromToDateJalaliValidation
    {
        public static (bool IsValid, string ErrorMessage) DateValidation(FromToDateJalaliDto input)
        {
            DateOnly? fromDate = input.FromDateJalai.ToGregorianDateOnly();
            DateOnly? toDate = input.ToDateJalai.ToGregorianDateOnly();

            if (!fromDate.HasValue && !toDate.HasValue)
                return (false, ExceptionLiterals.InvalidDate);

            if (toDate.Value < fromDate.Value)
                return (false, ExceptionLiterals.ToDateMoreThanFromDate);

            return (true, "");      
        }
    }
}
