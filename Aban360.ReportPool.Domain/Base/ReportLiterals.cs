namespace Aban360.ReportPool.Domain.Base
{
    public static class ReportLiterals
    {
        public static string Report { get { return "گزارش"; } }
        public static string CustomerSearch { get { return $"{Report} جستجوی مشترک"; } }
        public static string UnconfirmedSubscribers{ get { return $"{Report} مشترکین ثبت قطعی نشده"; } }
        public static string PrepaymentAndCalculation{ get { return $"{Report} ریز محاسبه و پیش پرداخت"; } }
        public static string CalculationDetails{ get { return $"{Report} ریز محاسبه"; } }
        public static string DeductionsAndDiscountsReport{ get { return $"{Report} کسورات و تخفیفات"; } }
        public static string DebtorByDay { get { return $"{Report} روزنامه بدهکاران"; } }//
        public static string LinkServiceStatement { get { return $"{Report} صورت وضعیت (سامانه فروش)"; } }
        public static string PendingPayments { get { return $"{Report} مشترکین بدهکار"; } }

        public static string UnspecifiedWaterPayment { get { return $"{Report} ردیف نامشخص آب بهاء"; } }
        public static string UnspecifiedServiceLinkPayment { get { return $"{Report} ردیف نامشخص حق انشعاب"; } }
        public static string DailyBankGrouped { get { return $"{Report}  به تفکیک بانک و روز"; } }
        public static string WaterPaymentDetail { get { return $"{Report} ریز وصولی ها - آب بهاء"; } }
        public static string ServiceLinkPaymentDetail { get { return $"{Report} ریز وصولی ها - حق انشعاب"; } }//
        public static string WaterPaymentReceivable { get { return $"{Report} وصولی های جاری و معوقه - آب بهاء"; } }
        public static string WaterUsageGrouped { get { return $"{Report} تفکیک کاربری - آب بهاء"; } }
        public static string Unpaid { get { return $"{Report} بدون پرداخت وصولی"; } }
        public static string WaterMeterReplacements { get { return $"{Report} گزاش کنتور های تعویضی"; } }
        public static string ModifiedBills { get { return $"{Report} "; } }//Todo: ادغام تسک 19 و 20
        public static string Unread { get { return $"{Report} بسته و مانع طی دوره"; } }
        public static string WithoutBill { get { return $"{Report} بدون صدور قبض"; } }
        public static string ContractualCapacity { get { return $"{Report} ظرفیت قراردادی"; } }
        public static string Usage { get { return $"{Report} باغ و اقامتگاه"; } }
        public static string HouseholdNumber { get { return $"{Report} خانوار"; } }
        public static string EmptyUnit { get { return $"{Report} خالی از سکنه"; } }
        public static string UsageDetail { get { return $"{Report} جزئیات کاربری"; } }
        public static string UsageSummary { get { return $"{Report} خلاصه گزارش"; } }
        public static string NonPermanentBranch { get { return $"{Report} انشعاب غیر دائم"; } }


        public static string SubscriptionEventSummary { get { return "کاردکس آب"; } }
        public static string WaterInstallationDetail { get { return "جزئیات انشعاب آب نصب شده"; } }
        public static string SewageInstallationDetail { get { return "جزئیات انشعاب فاضلاب نصب شده"; } }
        public static string WaterInstallationSummary{ get { return "خلاصه انشعاب آب نصب شده"; } }
        public static string SewageInstallationSummary { get { return "خلاصه انشعاب فاضلاب نصب شده"; } }

        public static string WaterRequestDetail { get { return "جزئیات انشعاب آب واگذار شده"; } }
        public static string SewageRequestDetail { get { return "جزئیات انشعاب فاضلاب واگذار شده"; } }
        public static string WaterRequestSummary { get { return "خلاصه انشعاب آب واگذار شده"; } }
        public static string SewageRequestSummary { get { return "خلاصه انشعاب فاضلاب واگذار شده"; } }

        public static string WaterRequestNonInstalledDetail { get { return "جزئیات انشعاب آب واگذار شده و نصب نشده"; } }
        public static string SewageRequestNonInstalledDetail { get { return " جزئیات انشعاب فاضلاب واگذار شده و نصب نشده"; } }
        public static string WaterRequestNonInstalledSummary { get { return "خلاصه انشعاب آب واگذار شده و نصب نشده"; } }
        public static string SewageRequestNonInstalledSummary { get { return "خلاصه انشعاب فاضلاب واگذار شده و نصب نشده"; } }
       
        public static string WithoutSewageRequestSummary { get { return "خلاصه انشعاب های بدون درخواست فاضلاب"; } }
        public static string WithoutSewageRequestDetail { get { return "جزئیات انشعاب های بدون درخواست فاضلاب"; } }

        public static string WaterDistanceRequestInstallationDetail { get { return "جزئیات فاصله واگذار تا نصب انشعاب آب"; } }
        public static string SewageDistanceRequesteInstallationDetail { get { return "جزئیات فاصله واگذار تا نصب انشعاب فاضلاب"; } }
        public static string WaterDistanceRequestInstallationSummary { get { return "خلاصه فاصله واگذار تا نصب انشعاب آب"; } }
        public static string SewageDistanceRequesteInstallationSummary { get { return "خلاصه فاصله واگذار تا نصب انشعاب فاضلاب"; } }

        public static string SendSmsToMobile { get { return $"{Report} پیامک ارسالی به مشترک"; } }

    }
}
