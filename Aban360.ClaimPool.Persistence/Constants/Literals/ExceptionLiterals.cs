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
        public static string InvalidInsertMother => "خطا در ذخیره اطلاعات مادر-فرزند";
        public static string InvalidUpdateMother => "خطا در ویرایش اطلاعات مادر-فرزند";
        public static string InvalidDeleteMother => "خطا در حذف اطلاعات مادر-فرزند";
        public static string InvalidUpdateToDayJalali => "خطا در ویرایش 'تا تاریخ'";
        public static string InvalidInsertArchmem=> "خطا در ذخیره تاریخچه مشترک";
        public static string InvalidUpdateBillAmount=> "خطا در ویرایش مبلغ قبض";
        public static string InvalidUpdateTrackNumber => "خطا در ویرایش شماره پیگیری";
        public static string InvalidInsertKart => "خطا در ذخیره اقلام محاسبه شده";
        public static string InvalidRemoveKart => "خطا در حذف قلم";
        public static string InvalidInsertGhest => "خطا در ذخیره اقساط";
        public static string InvalidDeleteGhest => "خطا در حذف اقساط";
        public static string InvalidUpdateGhest => "خطا در ویرایش اقساط";
        public static string InvalidInsertComment=> "خطا در ذخیره کامنت";

    }
}
