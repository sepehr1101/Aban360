namespace Aban360.PaymentPool.Application.Exceptions
{
    internal static class ExceptionLiterals
    {
        internal static string InvalidBankDocument(string message) => $"مقادیر سربرگ بانک با اطلاعات پرداختی کاربران، در تناقض است-{message}";
        internal static string InvalidTotalPrice => "قیمت کل اشتباه است";
        internal static string InvalidRecordCount=> "تعداد سطرها اشتباه است";
        internal static string InvalidBankId=> "کد بانک وارد شده اشتباه است";
    }
}
 