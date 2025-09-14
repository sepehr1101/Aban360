namespace Aban360.ReportPool.Domain.Base
{
    public static class ReportLiterals
    {
        public static string Report { get { return "گزارش"; } }
        public static string RegisterDate { get { return "تاریخ ثبت"; } }
        public static string ChangeDate { get { return "تاریخ تعویض"; } }
        public static string ByZone { get { return " بر اساس ناحیه "; } }
        public static string ByUsage { get { return " بر اساس کاربری "; } }
        public static string ByUsageAndZone { get { return " بر اساس کاربری و ناحیه "; } }
        public static string ByUsageAndZoneAndDiameter { get { return " بر اساس کاربری و ناحیه و قطر انشعاب "; } }
        public static string ByChangeCause { get { return "  بر اساس علت تعویض "; } }


        public static string EmptyUnit { get { return $"{Report} خالی از سکنه"; } }

        public static string EmptyUnitByBillDetail { get { return $"{Report} جزئیات از سکنه - قبض"; } }
        public static string EmptyUnitByBillSummary { get { return $"{Report} خلاصه خالی از سکنه قبض"; } }


        public static string ClientValidation { get { return $"{Report} صحت سنجی اطلاعات پایه"; } }

        public static string CustomerSearch { get { return $"{Report} جستجوی مشترک"; } }

        public static string UnconfirmedSubscribers { get { return $"{Report} مشترکین ثبت قطعی نشده"; } }

        public static string UseStateReport { get { return $"وضعیت انشعاب"; } }

        public static string PrepaymentAndCalculation { get { return $"{Report} پیش پرداخت - انشعاب"; } }
        public static string ServiceLinkCalculationDetails { get { return $"{Report} ریز محاسبه - انشعاب"; } }
        public static string WaterCalculationDetails { get { return $"{Report} ریز محاسبه - آب بها"; } }

        public static string WaterInvoice { get { return $"{Report} قبض"; } }

        public static string NonPermanentBranchDetail { get { return $"{Report} جرئیات انشعاب غیر‌دائم"; } }
        public static string NonPermanentBranchSummary { get { return $"{Report} خلاصه انشعاب غیر‌دائم"; } }

        public static string DeductionsAndDiscountsReportDetail { get { return $"{Report} جزئیات کسورات و تخفیفات"; } }
        public static string DeductionsAndDiscountsReportSummary { get { return $"{Report} خلاصه کسورات و تخفیفات"; } }

        public static string WithoutBill { get { return $"{Report} بدون صدور قبض"; } }
        public static string WithoutBillSummary { get { return $"{Report} خلاصه بدون صدور قبض"; } }

        public static string WaterIncomeAndConsumptionDetail { get { return $"{Report} جزئیات درآمد مصارف و آب‌بها"; } }
        public static string WaterIncomeAndConsumptionSummary { get { return $"{Report} خلاصه درآمد مصارف و آب‌بها"; } }
        //

        public static string ServiceLinkDebtorCustomers { get { return $"{Report} مانده مطالبات سررسید شده"; } }
        public static string DebtorByDayDetail { get { return $"{Report} جزئیات روزنامه بدهکاران"; } }
        public static string DebtorByDaySummary { get { return $"{Report} خلاصه روزنامه بدهکاران"; } }
        public static string LinkServiceStatement { get { return $"{Report} صورت وضعیت (سامانه فروش)"; } }
        public static string PendingPayments { get { return $"{Report} مشترکین بدهکار"; } }

        public static string UnspecifiedWaterPayment { get { return $"{Report} وصولی خارج سیستم _ آب بها"; } }
        public static string UnspecifiedServiceLinkPayment { get { return $"{Report} وصولی خارج سیستم - انشعاب"; } }
        public static string InvalidPayment { get { return $"{Report} وصولی نامعتبر"; } }
        public static string DailyBankGrouped { get { return $"{Report}  وصولی به تفکیک بانک و روز"; } }
        public static string WaterPaymentDetail { get { return $"{Report} ریز وصولی ها - آب بهاء"; } }
        public static string ServiceLinkPaymentDetail { get { return $"{Report} ریز وصولی ها - حق انشعاب"; } }
        public static string WaterPaymentReceivable { get { return $"{Report} وصولی های جاری و معوقه - آب بهاء"; } }
        public static string WaterUsageGrouped { get { return $"{Report} وصولی به تفکیک کاربری - آب بهاء"; } }
        public static string Unpaid { get { return $"{Report} بدون وصولی"; } }
        public static string WaterModifiedBillsDetail { get { return $"{Report}  جزئیات برگشتی و اصلاحات - آب بها"; } }
        public static string WaterModifiedBillsSummary { get { return $"{Report}  خلاصه برگشتی و اصلاحات - آب بها"; } }
        public static string ServiceLinkModifiedBillsSummary { get { return $"{Report} خلاصه برگشتی و اصلاحات - انشعاب"; } }
        public static string ServiceLinkModifiedBillsDetail { get { return $"{Report} جزئیات برگشتی و اصلاحات - انشعاب"; } }

        public static string RemovedBillSummary { get { return $"{Report} خلاصه قبوض ابطال شده"; } }
        public static string RemovedBillDetail { get { return $"{Report} جزئیات قبوض ابطال شده"; } }

        public static string Unread { get { return $"{Report} بسته و مانع طی دوره"; } }
        public static string UnreadSummary { get { return $"{Report} خلاصه بسته و مانع طی دوره"; } }

   

        public static string ContractualCapacity { get { return $"{Report} ظرفیت قراردادی"; } }
        public static string Usage { get { return $"{Report} باغ و اقامتگاه"; } }
        public static string HouseholdNumber { get { return $"{Report} خانوار"; } }
        public static string UsageDetail { get { return $"{Report} جزئیات کاربری"; } }
        public static string UsageSummary { get { return $"{Report} خلاصه کاربری"; } }
        public static string BranchTypeChangeHistory { get { return $"{Report} تاریخچه تغیرات نوع واگذاری"; } }
        public static string UsageChangeHistory { get { return $"{Report} تاریخچه تغیرات کاربری"; } }
        public static string DeletionStateChangeHistory { get { return $"{Report} تاریخچه تغیرات وضعیت انشعاب"; } }


        public static string WaterMeterReplacements(string reason) => $"{Report}  کنتور های تعویضی بر اساس {reason}";
        public static string WaterMeterReplacementsSummary(string reason) => $"- {Report} خلاصه کنتور های تعویضی بر اساس {reason}";

        public static string SubscriptionEventSummary { get { return "کاردکس آب"; } }
        public static string BranchEventSummary { get { return "کاردکس انشعاب"; } }
        public static string WaterInstallationDetail { get { return "جزئیات انشعاب آب نصب شده"; } }
        public static string SewageInstallationDetail { get { return "جزئیات انشعاب فاضلاب نصب شده"; } }
        public static string WaterInstallationSummary { get { return "خلاصه انشعاب آب نصب شده"; } }
        public static string SewageInstallationSummary { get { return "خلاصه انشعاب فاضلاب نصب شده"; } }
        public static string WaterInstallationSummaryByZoneId { get { return "خلاصه انشعاب آب نصب شده بر اساس ناحیه"; } }
        public static string SewageInstallationSummaryByZoneId { get { return "خلاصه انشعاب فاضلاب نصب شده بر اساس ناحیه"; } }

        public static string WaterRequestDetail { get { return "جزئیات انشعاب آب واگذار شده"; } }
        public static string SewageRequestDetail { get { return "جزئیات انشعاب فاضلاب واگذار شده"; } }
        public static string WaterRequestSummary { get { return "خلاصه انشعاب آب واگذار شده "; } }
        public static string SewageRequestSummary { get { return "خلاصه انشعاب فاضلاب واگذار شده "; } }

        public static string WaterRequestNonInstalledDetail { get { return "جزئیات انشعاب آب واگذار شده و نصب نشده"; } }
        public static string SewageRequestNonInstalledDetail { get { return " جزئیات انشعاب فاضلاب واگذار شده و نصب نشده"; } }
        public static string WaterRequestNonInstalledSummary { get { return "خلاصه انشعاب آب واگذار شده و نصب نشده"; } }
        public static string SewageRequestNonInstalledSummary { get { return "خلاصه انشعاب فاضلاب واگذار شده و نصب نشده"; } }

        public static string WithoutSewageRequestSummaryByZone { get { return "خلاصه انشعاب های بدون درخواست فاضلاب بر اساس ناحیه"; } }
        public static string WithoutSewageRequestSummary { get { return "خلاصه انشعاب های بدون درخواست فاضلاب"; } }
        public static string WithoutSewageRequestDetail { get { return "جزئیات انشعاب های بدون درخواست فاضلاب"; } }

        public static string WaterDistanceRequestRegisterDetail { get { return "جزئیات فاصله واگذار تا نصب انشعاب آب"; } }
        public static string SewageDistanceRequesteRegisterDetail { get { return "جزئیات فاصله واگذار تا نصب انشعاب فاضلاب"; } }
        public static string WaterDistanceRequestRegisterSummary { get { return "خلاصه فاصله واگذار تا نصب انشعاب آب"; } }
        public static string SewageDistanceRequesteRegisterSummary { get { return "خلاصه فاصله واگذار تا نصب انشعاب فاضلاب"; } }
        public static string WaterDistanceRequestRegisterSummaryByZone { get { return "خلاصه فاصله واگذار تا نصب انشعاب آب بر اساس ناحیه"; } }
        public static string SewageDistanceRequestRegisterSummaryByZone { get { return "خلاصه فاصله واگذار تا نصب انشعاب فاضلاب بر اساس ناحیه"; } }

        public static string WaterDistanceInstallationRegisterDetail { get { return "جزئیات فاصله ثبت تا نصب انشعاب آب"; } }
        public static string SewageDistanceInstallationeRegisterDetail { get { return "جزئیات فاصله ثبت تا نصب انشعاب فاضلاب"; } }
        public static string WaterDistanceInstallationRegisterSummary { get { return "خلاصه فاصله ثبت تا نصب انشعاب آب"; } }
        public static string SewageDistanceInstallationeRegisterSummary { get { return "خلاصه فاصله ثبت تا نصب انشعاب فاضلاب"; } }
        public static string WaterDistanceInstallationRegisterSummaryByZone { get { return "خلاصه فاصله ثبت تا نصب انشعاب آب بر اساس ناحیه"; } }
        public static string SewageDistanceInstallationRegisterSummaryByZone { get { return "خلاصه فاصله ثبت تا نصب انشعاب فاضلاب بر اساس ناحیه"; } }


        public static string SendSmsToMobile { get { return $"{Report} پیامک ارسالی به مشترک"; } }

        public static string HandoverDetail { get { return $"{Report} جزئیات نوع واگذاری"; } }
        public static string HandoverSummary { get { return $"{Report} خلاصه نوع واگذاری"; } }


        public static string ServiceLinkRawItemsDetail { get { return $"{Report} جزئیات جمع ناخالص اقلام انشعاب"; } }//
        public static string ServiceLinkRawItemsSummary { get { return $"{Report} خلاصه جمع ناخالص اقلام انشعاب"; } }

        public static string ServiceLinkNetItemsDetail { get { return $"{Report} جزئیات جمع خالص اقلام انشعاب"; } }//
        public static string ServiceLinkNetItemsSummary { get { return $"{Report} خلاصه جمع خالص اقلام انشعاب"; } }


        public static string MalfunctionToChangeDetail { get { return $"{Report} جزئیات کنتورهای خراب تعویض شده"; } }
        public static string MalfunctionToChangeSummary { get { return $"{Report} خلاصه کنتورهای خراب تعویض شده"; } }
        public static string MalfunctionMeterSummary { get { return $"{Report} خلاصه کنتورهای خراب"; } }
        public static string MalfunctionMeterDetail { get { return $"{Report} جزئیات کنتورهای خراب"; } }
        public static string MalfunctionMeterByDuration { get { return $"{Report} کنتورهای خراب بر اساس دوره"; } }
        public static string MalfunctionMeterByDurationSummary { get { return $"{Report} خلاصه کنتورهای خراب بر اساس دوره"; } }

        public static string WaterNetSalesDetail { get { return $"خلاصه {Report} جزئیات فروش خالص آب بها"; } }//
        public static string WaterRawSalesDetail { get { return $"خلاصه {Report} جزئیات فروش ناخالص آب بها"; } }
        public static string WaterNetSalesSummary { get { return $"خلاصه {Report} خلاصه فروش خالص آب بها"; } }
        public static string WaterRawSalesSummary { get { return $"خلاصه {Report} خلاصه فروش ناخالص آب بها"; } }

        public static string ExcessPattern { get { return $"خلاصه {Report} مازاد الگو"; } }
        public static string WaterNetIncome { get { return $"خلاصه {Report} درآمد خالص آب بها"; } }//
        public static string ReadingChecklist { get { return $" {Report} لیست قرائت و کنترل "; } }//

        
        public static string ReadingListDetail { get { return $"{Report} جزئیات فهرست تعداد قرائت"; } }//
        public static string ReadingListSummary { get { return $"{Report} خلاصه فهرست تعداد قرائت"; } }

        public static string ContractualAndOlgooLevel { get { return $"{Report} طبقات ظرفیت و الگو"; } }


        public static string ReadingDailyStatement { get { return $"خلاصه {Report} روزنامه"; } }//

        public static string ReadingStatusStatement { get { return $"خلاصه {Report} فهرست وضعیت"; } }
        public static string ReadingStatusStatementSummary { get { return $"خلاصه {Report} خلاصه قهرست وضعیت"; } }

        public static string RuinedMeterIncome { get { return $"خلاصه {Report} درآمد کنتور خراب"; } }
    }
}
