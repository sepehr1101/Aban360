namespace Aban360.ClaimPool.Persistence.Constants.Literals
{
    internal static class ExceptionLiterals
    {
        public static string InvalidIdentifier { get { return "موجودیت با شناسه ارسالی پیدا نشد"; } }
        public static string InvalidStateId => "وضعیت ارسالی یافت نشد.";
        public static string NotFoundTask => "وظیفه ای یافت نشد.";
        public static string InvalidInsertTracking => "خطا در ذخیره درخواست";
        public static string InvalidUpdateMoshtrakin=> "خطا در ویرایش اطلاعات مشترک";
        public static string InvalidInsertAssessment => "خطا در ذخیره نتیجه ارزیابی";
    }
}
