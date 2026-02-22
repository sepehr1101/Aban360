namespace Aban360.OldCalcPool.Persistence.Constants
{
    public static class Exceptionliterals
    {
        public static string InvalidUpdateWaterDebtAmount => "خطا در ویرایش مبلغ قبض";
        public static string InvalidBillInsert => "خطا در ذخیره قبض";
        public static string InvalidControUpdate => "خطا در ویرایش کنتور";
        public static string InvalidRemoveBill => "خطا در حذف قبض";
        public static string InvalidInsertBillHistory => "خطا در ذخیره تاریخچه قبض";
        public static string InvalidRemoveBill_ClosedVariab => "امکان حذف قبض برقرار نیست (حساب بسته شده)";
        public static string InvalidGhestAbInsert => "خطا در ذخیره اقساط آب‌بها";
        public static string InvalidDebtlessThan100000 => "مبلغ بدهی کمتر از 100.000تومان است.";
        public static string InvalidInsertMeterChange => "خطا در ذخیره تعویض";
        public static string InvalidDateLessThan2Month => "تاریخ نمی‌تواند کمتر از 2 ماه باشد.";
    }
}
