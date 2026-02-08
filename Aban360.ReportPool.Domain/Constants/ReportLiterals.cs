namespace Aban360.ReportPool.Domain.Base
{
    public static class ReportLiterals
    {
        public static string HandleFlat { get { return "HandleFlat"; } }
        public static string Handle { get { return "Handle"; } }

        public static string Report { get { return "گزارش"; } }
        public static string RegisterDate { get { return "تاریخ ثبت"; } }
        public static string ChangeDate { get { return "تاریخ تعویض"; } }
        public static string ByZone { get { return " بر اساس ناحیه "; } }
        public static string ZoneTitle { get { return "ZoneTitle"; } }
        public static string ByDay { get { return " بر اساس روز "; } }
        public static string ByUsage { get { return " بر اساس کاربری "; } }
        public static string UsageTitle { get { return "UsageTitle"; } }
        public static string ByUsageAndZone { get { return " بر اساس کاربری و ناحیه "; } }
        public static string ByCustomer { get { return " بر اساس مشترک "; } }
        public static string ByBill { get { return " بر اساس قبض "; } }
        public static string ByUsageAndZoneAndDiameter { get { return " بر اساس کاربری و ناحیه و قطر انشعاب "; } }
        public static string ByChangeCause { get { return "  بر اساس علت تعویض "; } }
        public static string ByChangeDate { get { return "  تاریخ تعویض "; } }
        public static string ByRegisterDate { get { return " تاریخ ثبت "; } }
        public static string Due { get { return "جاری"; } }
        public static string Overdue { get { return "معوقه"; } }
        public static string Olgoo { get { return "الگو"; } }
        public static string CotractualCapacity { get { return "ظرفیت قراردادی"; } }

        public static string WithZone { get { return " شهری "; } }
        public static string WithVillage { get { return " روستایی "; } }

        public static string EmptyUnit { get { return $"{Report} خالی از سکنه"; } }

        public static string EmptyUnitPossibility { get { return $"{Report} احتمال خالی از سکنه "; } }

        public static string EmptyUnitByBillDetail { get { return $"{Report} جزئیات خالی از سکنه - قبض"; } }
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
        public static string BankGrouped { get { return $"{Report}  وصولی به تفکیک بانک"; } }
        public static string WaterDailyBankGrouped { get { return $"{Report}  وصولی به تفکیک بانک و روز - آب بهاء"; } }
        public static string SewageDailyBankGrouped { get { return $"{Report}  وصولی به تفکیک بانک و روز - حق انشعاب"; } }
        public static string WaterPaymentDetail { get { return $"{Report} ریز وصولی ها - آب بهاء"; } }
        public static string ServiceLinkPaymentDetail { get { return $"{Report} ریز وصولی ها - حق انشعاب"; } }
        public static string WaterPaymentReceivableDetail { get { return $"{Report} جزئیات وصولی های جاری و معوقه - آب بهاء"; } }
        public static string WaterPaymentReceivableSummary { get { return $"{Report} خلاصه وصولی های جاری و معوقه - آب بهاء"; } }
        public static string ServiceLinkPaymentReceivableDetail { get { return $"{Report} جزئیات وصولی های جاری و معوقه - انشعاب"; } }
        public static string ServiceLinkPaymentReceivableSummary { get { return $"{Report} خلاصه وصولی های جاری و معوقه - انشعاب"; } }
        public static string WaterUsageGrouped { get { return $"{Report} وصولی به تفکیک کاربری - آب بهاء"; } }
        public static string WaterZoneGrouped { get { return $"{Report} وصولی به تفکیک ناحیه - آب بهاء"; } }

        public static string ServiceLinkUsageGrouped { get { return $"{Report} وصولی به تفکیک کاربری - انشعاب"; } }
        public static string ServiceLinkZoneGrouped { get { return $"{Report} وصولی به تفکیک ناحیه - انشعاب"; } }

        public static string Unpaid { get { return $"{Report} بدون وصولی"; } }
        public static string WaterModifiedBillsDetail { get { return $"{Report}  جزئیات برگشتی و اصلاحات - آب بها"; } }
        public static string WaterModifiedBillsSummary { get { return $"{Report}  خلاصه برگشتی و اصلاحات - آب بها"; } }
        public static string ServiceLinkModifiedBillsSummary { get { return $"{Report} خلاصه برگشتی و اصلاحات - انشعاب"; } }
        public static string ServiceLinkModifiedBillsDetail { get { return $"{Report} جزئیات برگشتی و اصلاحات - انشعاب"; } }

        public static string RemovedBillSummary { get { return $"{Report} خلاصه قبوض ابطال شده"; } }
        public static string RemovedBillDetail { get { return $"{Report} جزئیات قبوض ابطال شده"; } }

        public static string UnreadDetail { get { return $"{Report} جزئیات بسته و مانع طی دوره"; } }
        public static string UnreadSummary { get { return $"{Report} خلاصه بسته و مانع طی دوره"; } }


        public static string ReadingIssueDistanceBillDetail { get { return $"{Report} جزئیات فاصله قرائت تا صدور قبض "; } }
        public static string ReadingIssueDistanceBillSummary { get { return $"{Report} خلاصه فاصله قرائت تا صدور قبض"; } }



        public static string ContractualCapacity { get { return $"{Report} ظرفیت قراردادی"; } }
        public static string Usage { get { return $"{Report} باغ و اقامتگاه"; } }

        public static string HouseholdNumberDetail { get { return $"{Report} جزئیات خانوار"; } }
        public static string HouseholdNumberSummary { get { return $"{Report} خلاصه خانوار"; } }

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

        public static string WaterInstallationDepartmentSummary { get { return "خلاصه کارگاه نصب انشعاب آب "; } }
        public static string WaterInstallationDepartmentDetail { get { return "جزئیات کارگاه نصب انشعاب آب "; } }
        public static string SewageInstallationDepartmentSummary { get { return "خلاصه کارگاه نصب انشعاب فاضلاب "; } }
        public static string SewageInstallationDepartmentDetail { get { return "جزئیات کارگاه نصب انشعاب فاضلاب "; } }

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

        public static string WaterDistanceDeliverToInstallDetail { get { return "جزئیات فاصله تحویل کارگزار تا نصب"; } }
        public static string WaterDistanceDeliverToInstallSummary { get { return "خلاصه فاصله تحویل کارگزار تا نصب"; } }
        public static string WaterInstallGtRequest { get { return "انشعاب آب نصب شده زودتر از واگذار"; } }


        public static string SendSmsToMobile { get { return $"{Report} پیامک ارسالی به مشترک"; } }

        public static string HandoverDetail { get { return $"{Report} جزئیات نوع واگذاری"; } }
        public static string HandoverSummary { get { return $"{Report} خلاصه نوع واگذاری"; } }


        public static string ServiceLinkRawItemsDetail { get { return $"{Report} جزئیات جمع ناخالص اقلام انشعاب"; } }//
        public static string ServiceLinkRawItemsSummary { get { return $"{Report} خلاصه جمع ناخالص اقلام انشعاب"; } }

        public static string ServiceLinkNetItemsDetail { get { return $"{Report} جزئیات جمع خالص اقلام انشعاب"; } }//
        public static string ServiceLinkNetItemsSummary { get { return $"{Report} خلاصه جمع خالص اقلام انشعاب"; } }


        public static string MalfunctionToChangeDetail { get { return $"{Report} جزئیات کنتورهای خراب تعویض شده"; } }
        public static string MalfunctionToChangeSummary { get { return $"{Report} خلاصه کنتورهای خراب تعویض` شده"; } }
        public static string MalfunctionMeterSummary { get { return $"{Report} خلاصه کنتورهای خراب"; } }
        public static string MalfunctionMeterDetail { get { return $"{Report} جزئیات کنتورهای خراب"; } }
        public static string MalfunctionMeterByDurationDetail { get { return $"{Report} جزئیات کنتورهای خراب-دوره"; } }
        public static string MalfunctionMeterByDurationSummary { get { return $"{Report} خلاصه کنتورهای خراب-دوره"; } }
        public static string MalfunctionMeterByDurationGrowthSummary { get { return $"{Report} خلاصه رشد کنتورهای خراب-دوره"; } }

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


        public static string ReadingDailyStatement { get { return $" {Report} روزنامه"; } }//

        public static string ReadingStatusStatement { get { return $" {Report} فهرست وضعیت"; } }
        public static string ReadingStatusStatementSummary { get { return $" {Report} خلاصه فهرست وضعیت"; } }

        public static string RuinedMeterIncomeDetail { get { return $" {Report} جزئیات درآمد کنتور خراب"; } }
        public static string RuinedMeterIncomeSummary { get { return $" {Report} خلاصه درآمد کنتور خراب"; } }

        public static string MeterLifeDetail { get { return $" {Report} جزئیات عمر کنتور"; } }
        public static string MeterLifeSummary { get { return $" {Report} خلاصه عمر کنتور"; } }


        public static string TagGroupSummary { get { return $" {Report} خلاصه گروه تگ"; } }
        public static string TagGroupDetail { get { return $" {Report} جزئیات گروه تگ"; } }

        public static string TagSummary { get { return $" {Report} خلاصه تگ"; } }
        public static string TagDetail { get { return $" {Report} جزئیات تگ"; } }

        public static string ConsumptionManagerDetail { get { return $" {Report} جزئیات مدیریت مصرف"; } }
        public static string ConsumptionManagerSummary { get { return $" {Report} خلاصه مدیریت مصرف"; } }

        public static string LowWorkingMeter { get { return $" {Report} تحلیل متوسط مصرف"; } }
        public static string CustomerInfoWithSamePhoneNumber { get { return $" {Report} شناسه قبض های یک موبایل"; } }
        public static string LatestReadingBill { get { return $" {Report} آخرین اطلاعات قرائت"; } }

        public static string ClosedSummary { get { return $" {Report} فهرست وضعیت بسته"; } }

        public static string PaymentInquiry { get { return $" {Report} استعلام پرداختی"; } }
        public static string CustomerGeneralInfo { get { return " اطلاعات پایه "; } }

        public static string MeterDuplicateChangeDetail { get { return " جزئیات تعویض تکراری "; } }
        public static string MeterDuplicateChangeSummary { get { return " خلاصه تعویض تکراری "; } }

        public static string MeterReplacementLifeDetail { get { return " جزئیات عمر کنتورهای تعویض شده "; } }
        public static string MeterReplacementLifeSummary { get { return " خلاصه عمر کنتورهای تعویض شده "; } }

        public static string WaterSaleSummary { get { return " خلاصه فروش آب‌بها "; } }
        public static string WaterSaleDetail { get { return " جزئیات فروش آب‌بها "; } }

        public static string UspFinancial2 { get { return "گزارش درامد2"; } }
    }
}
