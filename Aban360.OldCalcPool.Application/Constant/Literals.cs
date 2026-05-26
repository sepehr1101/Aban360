namespace Aban360.OldCalcPool.Application.Constant
{
    public static class Literals
    {
        public static string InvalidDuration => "مدت زمان محاسبه نامعتبر";
        public static string GenerateBillOpLog => @"قبض صادر شد. شناسه قبض:{0}  شناسه پرداخت:{1}  مبلغ:{2}";
        public static string GenerateFreeBillOpLog => @"قبض با رقم قبلیصادر شد. شناسه قبض:{0}  شناسه پرداخت:{1}  مبلغ:{2}";
        public static string RemoveBillOpLog => @"قبض ابطال شد.  شناسه جدول:{0}  شناسه قبض:{1}.";
        public static string BillInstallmentOpLog => @"اقساط قبض انجام شد. شناسه قبض:{0}  مبلغ کل:{1}  تعداد اقساط:{2}  پیش پرداخت:{3}  درصد پیش پرداخت:{4}";
        public static string BillInstallmentManualOpLog => @"اقساط قبض انجام شد. شناسه قبض:{0}  مبلغ کل:{1}  تعداد اقساط:{2}  پیش پرداخت:{3}";
        public static string BillReturnOpLog => @"درخواست قبض برگشتی {0} انجام شد. شناسه قبض:{1}  تعداد قبض:{2}  از تاریخ:{3} تا تاریخ:{4}  مبلغ برگشتی:{5}  کد تایید:{6}";
        public static string BillReturnConfirmedOpLog => @"تایید نهایی قبض برگشتی انجام شد. شناسه قبض:{0}  تعداد قبض:{1}  از تاریخ:{2} تا تاریخ:{3}  مبلغ برگشتی:{4}  کد تایید:{5}";
        public static string TankerInsertOpLog => @"قبض آب تانکری توسط:{0}  صادر شد. تاریخ:{1} کد ناحیه:{2} شناسه قبض:{3} ردیف:{4} مبلغ:{5}";    
        public static string TankerDeleteOpLog => @"قبض آب تانکری توسط:{0}  حذف شد. تاریخ:{1} کد ناحیه:{2} شناسه قبض:{3} ردیف:{4} مبلغ:{5}";
        public static string RequestOfferingInsertOpLog => @"آیتم {0} برای شماره درخواست{1} ایجاد شد. مبلغ{2}  نوع آیتم {3}";
        public static string RequestOfferingReturnOpLog => @"برگشتی حق انشعاب انجام شد. شناسه قبض{0}  تعداد آیتم‌ها {1}  مبلغ{2}";
        public static string ServiceLinkRegisterManualOpLog => @"وصولی دستی حق انشعاب انجام شد. شناسه قبض:{0} تعداد پرداختی:{1}  جمع مبلغ:{2}";
    }
}
