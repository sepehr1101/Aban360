namespace Aban360.ReportPool.Domain.Base
{
    public static class ReportLiterals
    {
        public static string Report { get { return "گزارش"; } }
        public static string RegisterDate { get { return "تاریخ ثبت"; } }
        public static string ChangeDate { get { return "تاریخ تعویض"; } }

        public static string CustomerSearch { get { return $"{Report} جستجوی مشترک"; } }
        public static string UnconfirmedSubscribers{ get { return $"{Report} مشترکین ثبت قطعی نشده"; } }
        public static string PrepaymentAndCalculation{ get { return $"{Report} ریز محاسبه و پیش پرداخت"; } }
        public static string WaterCalculationDetails{ get { return $"{Report} ریز محاسبه - آب بها"; } }
        public static string ServiceLinkCalculationDetails{ get { return $"{Report} ریز محاسبه - انشعاب"; } }
        public static string DeductionsAndDiscountsReport{ get { return $"{Report} کسورات و تخفیفات"; } }
        public static string ServiceLinkDebtorCustomers{ get { return $"{Report} مانده مطالبات سررسید شده"; } }
        public static string DebtorByDayDetail { get { return $"{Report} جزئیات روزنامه بدهکاران"; } }
        public static string DebtorByDaySummary { get { return $"{Report} خلاصه روزنامه بدهکاران"; } }
        public static string LinkServiceStatement { get { return $"{Report} صورت وضعیت (سامانه فروش)"; } }
        public static string PendingPayments { get { return $"{Report} مشترکین بدهکار"; } }

        public static string UnspecifiedWaterPayment { get { return $"{Report} وصولی خارج سیستم _ آب بها"; } }
        public static string UnspecifiedServiceLinkPayment { get { return $"{Report} وصولی خارج سیستم-انشعاب"; } }
        public static string InvalidPayment { get { return $"{Report} وصولی نامعتبر"; } }
        public static string DailyBankGrouped { get { return $"{Report}  به تفکیک بانک و روز"; } }
        public static string WaterPaymentDetail { get { return $"{Report} ریز وصولی ها - آب بهاء"; } }
        public static string ServiceLinkPaymentDetail { get { return $"{Report} ریز وصولی ها - حق انشعاب"; } }//
        public static string WaterPaymentReceivable { get { return $"{Report} وصولی های جاری و معوقه - آب بهاء"; } }
        public static string WaterUsageGrouped { get { return $"{Report} تفکیک کاربری - آب بهاء"; } }
        public static string Unpaid { get { return $"{Report} بدون وصولی"; } }
        public static string WaterMeterReplacements(string reason)=> $"{Report} گزاش کنتور های تعویضی بر اساس {reason}"; 
        public static string WaterModifiedBillsDetail { get { return $"{Report}  جزئیات برگشتی و اصلاحات - آب بها"; } }
        public static string WaterModifiedBillsSummary { get { return $"{Report}  خلاصه برگشتی و اصلاحات - آب بها"; } }
        public static string ServiceLinkModifiedBillsSummary { get { return $"{Report} خلاصه برگشتی و اصلاحات - انشعاب"; } }
        public static string ServiceLinkModifiedBillsDetail { get { return $"{Report} جزئیات برگشتی و اصلاحات - انشعاب"; } }
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
        public static string BranchEventSummary { get { return "کاردکس انشعاب"; } }
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

        public static string HandoverDetail { get { return $"{Report} جزئیات نوع واگذاری"; } }
        public static string HandoverSummary { get { return $"{Report} خلاصه نوع واگذاری"; } }


        public static string WaterCollectionBranchDetail { get { return "جزئیات انشعاب آب جمع آوری شده"; } }
        public static string SewageCollectionBranchDetail { get { return "جزئیات انشعاب فاضلاب جمع آوری شده"; } }
        public static string WaterCollectionBranchSummary { get { return "خلاصه انشعاب آب جمع آوری شده"; } }
        public static string SewageCollectionBranchSummary { get { return "خلاصه انشعاب فاضلاب جمع آوری شده"; } }


        public static string ServiceLinkRawItemsDetail { get { return $"{Report} جزئیات جمع ناخالص اقلام انشعاب"; } }
        public static string ServiceLinkRawItemsSummary { get { return $"{Report} خلاصه جمع ناخالص اقلام انشعا"; } }

        public static string ServiceLinkNetItemsDetail { get { return $"{Report} جزئیات جمع خالص اقلام انشعاب"; } }
        public static string ServiceLinkNetItemsSummary { get { return $"{Report} خلاصه جمع خالص اقلام انشعا"; } }


        public static string MalfunctionMeter { get { return $"{Report} کنتورهای خراب"; } }
        public static string MalfunctionMeterByDuration { get { return $"{Report} کنتورهای خراب بر اساس دوره"; } }

        public static string WaterNetSalesDetail { get { return $"خلاصه {Report} جزئیات فروش خالص آب بها"; } }
        public static string WaterRawSalesDetail { get { return $"خلاصه {Report} جزئیات فروش ناخالص آب بها"; } }
        public static string WaterNetSalesSummary { get { return $"خلاصه {Report} خلاصه فروش خالص آب بها"; } }
        public static string WaterRawSalesSummary { get { return $"خلاصه {Report} جزئیات فروش ناخالص آب بها"; } }

        public static string ExcessPattern { get { return $"خلاصه {Report} مازاد الگو"; } }
        public static string WaterNetIncome { get { return $"خلاصه {Report} درآمد خالص آب بها"; } }
        public static string ReadingChecklist{ get { return $"خلاصه {Report} لیست کنترا قرائت"; } }


        public static string ReadingListDetail { get { return $"خلاصه {Report} جزئیات فهرست تعداد قرائت"; } }
        public static string ReadingListSummary { get { return $"خلاصه {Report} خلاصه فهرست تعداد قرائت"; } }

        public static string ReadingDailyStatement { get { return $"خلاصه {Report} روزنامه"; } }
        public static string ReadingStatusStatement { get { return $"خلاصه {Report} فهرست وضعیت"; } }
    }
}
