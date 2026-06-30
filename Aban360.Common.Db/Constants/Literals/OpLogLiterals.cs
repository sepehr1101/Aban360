namespace Aban360.Common.Db.Constants.Literals
{
    public static class OpLogLiterals
    {
        public static string GenerateBillIssueRemianedOpLog => @"قبض تسویه صادر شد. شناسه قبض:{0}  شناسه پرداخت:{1}  مبلغ:{2}";
        public static string GenerateBatchBillOpLog => @"قبض دسته‌ای صادر شد. ناحیه:{0}  تعداد قبض:{1}";
        public static string GenerateBillOpLog => @"قبض صادر شد. شناسه قبض:{0}  شناسه پرداخت:{1}  مبلغ:{2}";
        public static string GenerateFreeBillOpLog => @"قبض با رقم قبلیصادر شد. شناسه قبض:{0}  شناسه پرداخت:{1}  مبلغ:{2}";
        public static string RemoveBillOpLog => @"قبض ابطال شد.  شناسه جدول:{0}  شناسه قبض:{1}.";
        public static string BillInstallmentOpLog => @"اقساط قبض انجام شد. شناسه قبض:{0}  مبلغ کل:{1}  تعداد اقساط:{2}  پیش پرداخت:{3}  درصد پیش پرداخت:{4}";
        public static string BillInstallmentUpdateOpLog => @"ویرایش قسط آب‌بها انجام شد. شناسه قبض:{0} شناسه قسط:{1} مبلغ قبلی:{2}  مبلغ فعلی:{3}  مهلت قبلی:{4}  مهلت فعلی:{5}";
        public static string BillInstallmentManualOpLog => @"اقساط قبض انجام شد. شناسه قبض:{0}  مبلغ کل:{1}  تعداد اقساط:{2}  پیش پرداخت:{3}";
        public static string BillReturnOpLog => @"درخواست قبض برگشتی {0} انجام شد. شناسه قبض:{1}  تعداد قبض:{2}  از تاریخ:{3} تا تاریخ:{4}  مبلغ برگشتی:{5}  کد تایید:{6}";
        public static string BillReturnConfirmedOpLog => @"تایید نهایی قبض برگشتی انجام شد. شناسه قبض:{0}  تعداد قبض:{1}  از تاریخ:{2} تا تاریخ:{3}  مبلغ برگشتی:{4}  کد تایید:{5}";
        public static string BillReturnDeletedOpLog => @"قبض برگشتی تایید نشده، حذف شد. شناسه قبض:{0}  از تاریخ:{1}   تا تاریخ:{2}   کد تایید:{3}";
        public static string TankerInsertOpLog => @"قبض آب تانکری توسط:{0}  صادر شد. تاریخ:{1} کد ناحیه:{2} شناسه قبض:{3} ردیف:{4} مبلغ:{5}";
        public static string TankerDeleteOpLog => @"قبض آب تانکری توسط:{0}  حذف شد. تاریخ:{1} کد ناحیه:{2} شناسه قبض:{3} ردیف:{4} مبلغ:{5}";
        public static string RequestOfferingInsertOpLog => @"آیتم {0} برای شماره درخواست{1} ایجاد شد. مبلغ{2}  نوع آیتم {3}";
        public static string ServiceLinkReturnOpLog => @"برگشتی حق انشعاب انجام شد. شناسه قبض{0}  مبلغ{1}";
        public static string ServiceLinkReturnDisconnectOpLog => @"برگشتی حق انشعاب-برچیدن انشعاب انجام شد. شناسه قبض{0}  مبلغ{1}";
        public static string ServiceLinkRegisterManualOpLog => @"وصولی دستی حق انشعاب انجام شد. شناسه قبض:{0}  مبلغ:{1}";
        public static string ServiceLinkDeleteManualOpLog => @"حذف وصولی دستی حق انشعاب انجام شد. شناسه قبض:{0}  مبلغ:{1}";
        public static string ServiceLinkReturnRemoved => @"قبض برگشتی حق انشعاب حذف شد. شناسه قبض:{0}  تاریخ:{1}  مبلغ:{2}  نوع قبض:{3}  نوع درخواست:{4}";
        public static string ServiceLinkReturnConfirmedOpLog => @"برگشتی دسته‌ای حق انشعاب انجام شد. شناسه قبض:{0}  تعداد برگشتی:{1}";

        public static string ServiceLinkConnectInsertOpLog => @"صدور دستور وصل انجام شد. شناسه قبض:{0} ";
        public static string ServiceLinkDisconnectInsertOpLog => @"صدور دستور قطع انجام شد. شناسه قبض:{0}  علت:{1}";
        public static string ServiceLinkConnectSetResultOpLog => @"ثبت نتیجه وصل انجام شد. شناسه قبض:{0}";
        public static string ServiceLinkDisconnectSetResultOpLog => @"ثبت نتیجه قطع انجام شد. شناسه قبض:{0}  نتیجه:{1}";
        public static string ServiceLinkConnectRemoveOpLog => @"دستور وصل حذف شد. شناسه قبض:{0}  شناسه جدول:{1}";
        public static string ServiceLinkDisconnectRemoveOpLog => @"دستور قطع حذف شد. شناسه قبض:{0}  شناسه جدول:{1}";
        public static string ConCompanyInsertOpLog => @"پیمانکار قطع/وصل ایجاد شد. نام شرکت:{0}  نام نماینده:{1}";
        public static string ConCompanyUpdateOpLog => @"پیمانکار قطع/وصل ویرایش شد. نام شرکت:{0}  نام نماینده:{1}";
        public static string ConCompanyRemoveOpLog => @"پیمانکار قطع/وصل حذف شد. نام شرکت:{0}  نام نماینده:{1}";
        public static string ConCompanyPersonnelInsertOpLog => @"مامور وصول پیمانکار قطع/وصل ایجاد شد. نام مامور:{0}  کدملی:{1}";
        public static string ConCompanyPersonnelUpdateOpLog => @"مامور وصول پیمانکار قطع/وصل ویرایش شد. نام مامور:{0}  کدملی:{1}";
        public static string ConCompanyPersonnelRemoveOpLog => @"مامور وصول پیمانکار قطع/وصل حذف شد. نام مامور:{0}  کدملی:{1}";
        public static string JudicalNoticeCommandInsertOpLog => @"دستور اسناد رسمی ایجاد شد. شناسه قبض:{0}  مبلغ بدهی:{1}  نام شرکت:{2}  نام نماینده:{3}";

        public static string UserCreate => @"کاربر ایجاد شد. نام کامل:{0}  نام کاربری:{1}  تلفن همراه:{2}"; 
        public static string UserUpdate => @"کاربر ویرایش شد. شناسه کاربر:{0}  نام کامل :از {1} به {2}  نام کاربری:از {3} به {4}  تلفن همراه:از {5} به {6}"; 
        public static string UserDelete => @"کاربر حذف شد. شناسه کاربر:{0}"; 
     
        public static string MeterFlowRemoveOpLog => @"فایل قرائت حذف شد. نام قبلی فایل:{0}  نام فعلی فایل:{1} ناحیه:{2}"; 
     
    }
}
