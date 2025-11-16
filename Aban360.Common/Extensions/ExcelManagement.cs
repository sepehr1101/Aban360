using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using DNTPersianUtils.Core;
using MiniExcelLibs;
using System.Reflection;

namespace Aban360.Common.Extensions
{
    public static class ExcelManagement
    {
        public static string pathBase { get; set; } = "AppData\\Excels\\";
        public static int maxDataCount { get; set; } = 1000000;
        public static async Task<string> ExportToExcelAsync<THeader, TData>(THeader tHeader, IEnumerable<TData> tData, string reportName, string[]? excludedProperties = null)
        {
            Validate(tHeader, tData);

            var excelfile = new Dictionary<string, object>();
            int sheetCount = tData.Count() / maxDataCount;


            excelfile[ExceptionLiterals.Header] = new List<Dictionary<string, object>> { TranslateHeader(tHeader) };
            for (int i = 0; i < sheetCount + 1; i++)
            {
                var sheetData = tData.Skip(i * maxDataCount).Take(maxDataCount).ToList();
                excelfile[ExceptionLiterals.Page(i + 1)] = TranslateData(sheetData,excludedProperties);
            }

            string path = GetPath(reportName);
            try
            {
                await MiniExcel.SaveAsAsync(path, excelfile);

                return path;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private static string GetPath(string reportName)
        {
            string reportTitle = reportName
                          .Replace('/', '-')
                          .Replace(':', '-')
                          .Replace(' ', '_');


            var timeNow = DateTime.Now.ToString("HH-mm-ss");
            var persianDate = DateTime
                                        .Now
                                        .ToShortPersianDateString()
                                        .Replace('/', '-')
                                        .Replace(':', '-')
                                        .Replace(' ', '_');

            return $"{pathBase}{reportTitle}_{persianDate}_{timeNow}.xlsx";
        }
        private static void Validate<THeader, TData>(THeader tHeader, IEnumerable<TData> tData)
        {
            tData.NotNull(nameof(tData));
            if (tHeader == null || tData == null)
                throw new BaseException(ExceptionLiterals.CantGenarateExcelWithNullData);
        }

        private static Dictionary<string, object> TranslateHeader<T>(T data)
        {
            var props = GetOrderedProperties(data);
            var persianProp = GetPersianProperty();
            var row = new Dictionary<string, object>();
            foreach (var prop in props)
            {
                var propName = persianProp?.ContainsKey(prop.Name) == true ? persianProp[prop.Name] : prop.Name;
                row[propName] = prop.GetValue(data);
            }
            return row;
        }

        private static List<Dictionary<string, object>> TranslateData<T>(IEnumerable<T> data, string[]? excludedProperties = null)
        {
            var result = new List<Dictionary<string, object>>();

            var enumerator = data.GetEnumerator();
            if (!enumerator.MoveNext()) return result;

            var first = enumerator.Current;
            string[]? excludedProps=excludedProperties ?? Array.Empty<string>();
            var props = GetOrderedProperties(first).Where(p=>!excludedProps.Contains(p.Name)).ToList();
            var persianProp = GetPersianProperty();
            do
            {
                var row = new Dictionary<string, object>();
                foreach (var prop in props)
                {
                    var propName = persianProp?.ContainsKey(prop.Name) == true ? persianProp[prop.Name] : prop.Name;
                    row[propName] = prop.GetValue(enumerator.Current) ?? "";
                }
                result.Add(row);
            }
            while (enumerator.MoveNext());

            return result;
        }
        private static List<PropertyInfo> GetOrderedProperties(object obj)
        {
            return obj.GetType()
                      .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                      .OrderBy(p => p.MetadataToken)
                      .ToList();
        }
        private static Dictionary<string, string> GetPersianProperty()
        {
            return new Dictionary<string, string>()
            {
                    {"CustomerNumber", "شماره ردیف"},
                    {"ReadingNumber", "شماره اشتراک"},
                    {"FirstName", "نام"},
                    {"Surname", "نام خانوادگی"},
                    {"FullName", "نام و نام خانوادگی"},
                    {"UsageTitle", "نوع کاربری"},
                    {"MeterDiameterTitle", "قطر کنتور"},
                    {"EventDateJalali", "تاریخ رویداد"},
                    {"Address", "آدرس"},
                    {"SumDomesticUnit", "جمع واحد مسکونی"},
                    {"DomesticUnit", "واحد مسکونی"},
                    {"SumCommercialUnit", "جمع واحد تجاری"},
                    {"CommercialUnit", "واحد تجاری"},
                    {"SumOtherUnit", "جمع سایر واحدها"},
                    {"OtherUnit", "سایر واحدها"},
                    {"BillId", "شناسه قبض"},
                    {"UseStateTitle", "نوع واگذاری"},
                    {"EmptyUnit", "واحد خالی"},
                    {"ZoneId", "کد منطقه"},
                    {"ZoneTitle", "نام منطقه"},
                    {"RegionId", "کد ناحیه"},
                    {"RegionTitle", "عنوان ناحیه"},
                    {"NationalCode", "کد ملی"},
                    {"PostalCode", "کد پستی"},
                    {"PhoneNumber", "شماره تلفن"},
                    {"MobileNumber", "شماره تلفن"},
                    {"FatherName", "نام پدر"},
                    {"FromReadingNumber", "از شماره اشتراک"},
                    {"ToReadingNumber", "تا شماره اشتراک"},
                    {"FromDateJalali", "از تاریخ"},
                    {"ToDateJalali", "تا تاریخ"},
                    {"ReportDateJalali", "تاریخ گزارش"},
                    {"ReportCount", "تعداد سطر"},
                    {"RecordCount", "تعداد سطر"},
                    {"SumDomesticCount", "جمع آحاد تجاری"},
                    {"SumCommercialCount", "جمع آحاد مسکونی"},
                    {"SumOtherCount", "جمع آحاد سایر"},
                    {"TotalUnit", "آحاد کل"},
                    {"SumEmptyUnit", "جمع آحاد خالی"},
                    {"FromEmptyUnit", "از واحد خالی"},
                    {"ToEmptyUnit", "تا واحد خالی"},
                    {"IsFirstRow", "*"},
                    {"ContractualCapacity", "ظرفیت قراردادی"},
                    {"MeterInstallationDateJalali", "تاریخ نصب آب"},
                    {"BranchType", "نوع واگذاری"},
                    {"SiphonDiameterTitle", "قطر سیفون"},
                    {"ConsumptionAverage", "میانگین مصرف"},
                    {"Consumption", "مصرف"},
                    {"MalfunctionPeriodCount", "تعداد دوره خراب"},
                    {"LastChangeDateJalali", "تاریخ آخرین تغیرات"},
                    {"MeterRequestDateJalali", "تاریخ درخواست"},
                    {"DeletionStateTitle", "وضعیت انشعاب"},
                    {"SumItems", "جمع"},
                    {"MeterLife", "عمر کنتور"},
                    {"Duration", "دوره"},
                    {"LastReadingDay", "تاریخ آخرین قرائت"},
                    {"LatestChangeDateJalali", "تاریخ آخرین تغیرات"},
                    {"Payable", "قابل پرداخت"},
                    {"TotalPayable", "جمع قابل پرداخت"},
                    {"CustomerCount", "تعداد مشترک"},
                    {"Title", "عنوان"},
                    {"FromMalfunctionPeriodCount", "از تعداد دوره خراب"},
                    {"ToMalfunctionPeriodCount", "تا تعداد دوره خراب"},
                    {"HasSewage", "آیا فاضلاب دارد"},
                    {"SewageInstallationDateJalali", "تاریخ نصب فاضلاب"},
                    {"CommercialArea", "تعداد آحاد تجاری"},
                    {"DomesticArea", "تعداد آحاد مسکونی"},
                    {"ConstructedArea", "اعیان سایر"},
                    {"Description", "توضیحات"},
                    {"ChangeDateJalali", "تاریخ تعویض"},
                    {"Distance", "فاصله"},
                    {"DistanceText", "فاصله"},
                    {"FromDeletionStateTitle", "از وضعیت انعشاب"},
                    {"ToDeletionStateTitle", "تا وضعیت انشعاب"},
                    {"FromBranchType", "از نوع واگذاری"},
                    {"ToBranchType", "تا نوع واگذاری"},
                    {"FromUsageTitle", "از کاربری"},
                    {"ToUsageTitle", "تا کاربری"},
                    {"FromState", "از وضعیت"},
                    {"ToState", "تا وضعیت"},
                    {"OldCustomerNumber", "شماره ردیف قبلی"},
                    {"OldBillId", "شناسه قبض قبلی"},
                    {"VillageId", "کد روستا"},
                    {"VillageName", "نام روستا"},
                    {"SpecialCustomer", "خاص"},
                    {"CommonSiphon", "سیفون مشترک"},
                    {"UnSpecified", "قطر نامشخص"},
                    {"Field0_5", "1/2"},
                    {"Field0_75", "3/4"},
                    {"Field1", "1"},
                    {"Field1_2", "1.2"},
                    {"Field1_5", "1.5"},
                    {"Field2", "2"},
                    {"Field3", "3"},
                    {"Field4", "4"},
                    {"Field5", "5"},
                    {"MoreThan6", "6 و بالاتر"},
                    {"Count", ""},
                    {"RegisterDayJalali", "تاریخ ثبت انشعاب آب"},
                    {"HouseholdDateJalali", "از تاریخ خانواری"},
                    {"HouseholdCount", "تعداد خانواری"},
                    {"IsValid", "وضعیت اعتبار"},
                    {"SumHousehold", "مجموع خانواری"},
                    {"Field6", "6"},
                    {"FieldMore5", "5 و بالاتر"},
                    {"WaterRequestDateJalali", "تاریخ درخواست انشعاب آب"},
                    {"WaterInstallationDate", "تاریخ نصب آب"},
                    {"DebtAmount", "بدهکار"},
                    {"MeterRequestDate", "تاریخ درخواست آب"},
                    {"MeterInstallationDate", "تاریخ نصب آب"},
                    {"FromHouseholdDateJalali", "از تاریخ خانواری"},
                    {"ToHouseholdDateJalali", "تا تاریخ خانواری"},
                    {"FromContractualCapacity", "از ظرفیت قراردادی"},
                    {"ToContractualCapacity", "تا ظرفیت قراردادی"},
                    {"FinalAmount", "مبلغ"},
                    {"PreInstallmentAmount", "مبلغ قسط"},
                    {"SumFinalAmount", "مبلغ کل"},
                    {"SumPreInstallmentAmount","جمع اقساط"},
                    {"Mobile", "شماره موبایل"},
                    {"TrackNumber", "شماره پیگری"},
                    {"RequestDateJalali", "تاریخ درخواست"},
                    {"Id", "شماره"},
                    {"IsVillage", "روستا"},
                    {"BankName", "نام بانک"},
                    {"BankCode", "کد بانک"},
                    {"WaterCount", "تعداد آب"},
                    {"WaterAmount", "مبلغ آب"},
                    {"ServiceLinkAmount", "مبلغ فاضلاب"},
                    {"ServiceLinkCount", "تعداد فاضلاب"},
                    {"TotalAmount", "مبلغ کل"},
                    {"TotalCount", "جمع تعداد"},
                    {"FromBankId", "از کد بانک"},
                    {"ToBankId", "تا کد بانک"},
                    {"RegisterDate", "تاریخ ثبت"},
                    {"BankDate", "تاریخ بانک"},
                    {"ItemCount", "تعداد"},
                    {"ItemAmount", "مبلغ"},
                    {"FromAmount", "از مبلغ"},
                    {"ToAmount", "تا مبلغ"},
                    {"OffAmount", "تخفیف"},
                    {"SumAmount", "مبلغ کل"},
                    {"SumOffAmount", "جمع تخفیف"},
                    {"BankAbbriviation", "خلاصه نام بانک"},
                    {"CheckState", "وضعیت"},
                    {"BankDateJalali", "تاریخ بانک"},
                    {"EventBankDateJalali", "آخرین تاریخ بانک"},
                    {"PaymentMethodTitle", "روش پرداخت"},
                    {"PaymentDate", "تاریخ پرداخت"},
                    {"PayId", "شناسه پرداخت"},
                    {"Amount", "مبلغ"},
                    {"AverageConsumption", "متوسط مصرف"},
                    {"SumConsumption", "مجموع مصرف"},
                    {"UsageSellTitle", "کابری فروش"},
                    {"PeriodCount", "تعداد دوره"},
                    {"BankId", "شماره بانک"},
                    {"PaymentId", "شماره پرداخت"},
                    {"PaymentGateway", "روش پرداخت"},
                    {"DueAmount", "مبلغ جاری"},
                    {"OverdueAmount", "مبلغ معوق"},
                    {"DueCount", "تعداد جاری"},
                    {"OverdueCount", "تعداد معوق"},
                    {"BillCount", "تعداد قبض"},
                    {"AmountState", "جاری/ معوق"},
                    {"FileName", "نام فایل"},
                    {"FieldArea", "اعیان کل"},
                    {"HeadquarterTitle", "نام منطقه خودگردان"},
                    {"CurrentMeterNumber", "شماره کنتور فعلی"},
                    {"PreviousMeterNumber", "شماره کنتور قبلی"},
                    {"CurrentDateJalali", "تاریخ قرائت فعلی"},
                    {"PreviousDateJalali", "تاریخ قرائت قبلی"},
                    {"RemovedDateJalali", "تاریخ حذف"},
                    {"DebtPeriodCount", "تعداد دوره بدهی"},
                    {"BeginDebt", "شروغ بدهی"},
                    {"EndingDebt", "پایان بدهی"},
                    {"PayedAmount", "مبلغ پرداخت"},
                    {"TotalDebtPeriodCount", "تعداد دوره بدهی کل"},
                    {"TotalBeginDebt", "شروع بدهی"},
                    {"TotalEndingDebt", "پایان بدهی"},
                    {"TotalPayedAmount", "پرداخت کل"},
                    {"ZoneCount", "تعداد ناحیه"},
                    {"UsageConsumptionTitle", "کاربری مصرف"},
                    {"DiscountTypeTitle", "نوع تخفیف"},
                    {"OffTypeTitle", "نوع تخفیف"},
                    {"DiscountTitle", "تخفیف"},
                    {"InheritedPrimises", "سهم رسیده عرصه"},
                    {"InheritedImprovementOverall", "سهم رسیده اعیانی کل"},
                    {"InheritedImprovementCommericial", "سهم رسیده اعیانی تجاری"},
                    {"InheritedImprovementDomestic", "سهم رسیده اعیانی مسکونی"},
                    {"InheritedImprovementOther", "سهم رسیده اعیانی سایر"},
                    {"InheritedUnitCommericial", "سهم رسیده آحاد تجاری"},
                    {"InheritedUnitDomestic", "سهم رسیده آحاد مسکونی"},
                    {"InheritedUnitOther", "سهم رسیده آحاد کل"},
                    {"InheritedContractualCapacity", "سهم رسیده ظرفیت قراردادی"},
                    {"WaterInstallDateJalali", "تاریخ نصب انشعاب آب"},
                    {"Primises", "عرصه"},
                    {"ImprovementOverall", "اعیانی کل"},
                    {"ImprovementCommericial", "اعیانی تجاری"},
                    {"ImprovementDomestic", "اعیانی مسکونی"},
                    {"ImprovementOther", "اعیانی سایر"},
                    {"IssueDateJalali", "تاریخ صدور"},
                    {"UnitCommericial", "آحاد تجاری"},
                    {"UnitDomestic", "آحاد مسکونی"},
                    {"UnitOther", "آحاد سایر"},
                    {"SumPremisesImprovement", "جمع عرصه و اعیان"},
                    {"InheritedFromCustomerNumber", "سهم رسیده از شماره ردیف"},
                    {"TypeTitle", ""},
                    {"ItemTitle", "عنوان"},
                    {"ItemId", "کد"},
                    {"InstallmentNumber", "شمراه قسط"},
                    {"InstallmentCount", "تعداد قسط"},
                    {"CustomerHeader", ""},
                    {"InstallmentHeader", ""},
                    {"SumItemsAmount", "جمع مبلغ"},
                    {"SumItemsDiscount", "جمع قسط"},
                    {"DebtorOrCreditorAmount", "بستانکار یا بدهکار"},
                    {"PersianStringAmount", "مبلغ به حروف"},
                    {"PaymentDateJalali", "تاریخ پرداخت"},
                    {"PaymentGetway", "روش پرداخت"},
                    {"Barcode", "بارکد"},
                    {"BranchTypeTitle", "نوع واگذاری"},
                    {"BranchTypeId", "کد نوع واگذاری"},
                    {"PageNumber", "شماره صفحه"},
                    {"RequestNumber", "شماره درخواست"},
                    {"UnitCommeicial", "آحاد تجاری"},
                    {"ImprovementsCommericial", "اعیانی تجاری"},
                    {"ImprovementsDomestic", "اعیانی مسکونی"},
                    {"ImprovementsOverall", "اعیانی کل"},
                    {"ReadingBlock", "بلوک دارایی"},
                    {"ServiceDescription", "توضیحات"},{"PaymentDetail", ""},
                    {"Prepayment", "شماره درخواست"},
                    {"PreviousNumber", "شماره قبلی"},
                    {"NextNumber", "شماره فعلی"},
                    {"RegisterDateJalali", "تاریخ ثبت"},
                    {"CounterStateTitle", "وضعیت فعلی"},
                    {"WaterDiameterId", "کد قطر کنتور"},
                    {"WaterDiameterTitle", "قطر کنتور"},
                    {"ReadingStateTitle", "وضعیت قرائت"},
                    {"AverageAll", "میانگین کل"},
                    {"AverageAllText", "میانگین کل"},
                    {"UnSpecifiedText", "قطر نامشخص"},
                    {"Field0_5Text", "1/2"},
                    {"Field0_75Text", "3/4"},
                    {"Field1Text", "1"},
                    {"Field1_2Text", "1.2"},
                    {"Field1_5Text", "3"},
                    {"Field2Text", "2"},
                    {"Field3Text", "3"},
                    {"Field4Text", "4"},
                    {"Field5Text", "5"},
                    {"MoreThan6Text", "6 و بالاتر"},
                    {"DiscountAmount", "تخفیف"},
                    {"InstallmentAmount", "مبلغ قسط"},
                    {"SumNetAmount", "جمع مبلغ خالص"},
                    {"SumRawAmount", "جمع مبلغ ناخالص"},
                    {"CurrentPrimises", "اطلاعات فعلی عرصه "},
                    {"CurrentImprovementOverall", "اطلاعات فعلی اعیانی کل"},
                    {"CurrentImprovementCommericial", "اطلاعات فعلی اعیانی تجاری"},
                    {"CurrentImprovementDomestic", "اطلاعات فعلی  اعیانی مسکونی"},
                    {"CurrentImprovementOther", "اطلاعات فعلی سایر"},
                    {"CurrentUnitCommericial", "اطلاعات فعلی آحاد تجاری"},
                    {"CurrentUnitDomestic", "اطلاعات فعلی آحاد مسکونی"},
                    {"CurrentUnitOther", "اطلاعات فعلی آحاد سایر"},
                    {"CurrentContractualCapacity", "اطلاعات فعلی ظرفیت قراردادی"},
                    {"SumCurrentPremisesImprovement", "اطلاعات فعلی عرصه اعیانی"},
                    {"CurrentItems", "اطلاعات فعلی"},
                    {"InheritedItems", "سهم رسیده"},
                    {"PreviousItems", "اطلاعات قبلی"},
                    {"SumItemAmount", "مبلغ کل"},
                    {"InstallmentDebtAmount", "مجموع بدهی"},
                    {"CreditorAmount", "جمع بستانکار"},
                    {"PrincipalDebt", "بدهی اصلی"},
                    {"TotalDebt", "مجموعت بدهی"},
                    {"SumInstallmentDebtAmout", "مجموع بدهی"},
                    {"SumCreditAmount", "جمع بستانکار"},
                    {"SumPrincipalDebt", "بدهکار اصلی"},
                    {"SumTotalDebt", "کل بدهی"},
                    {"RequestDate", "تاریخ درخواست آب"},
                    {"InstallationDate", "تاریخ نصب آب"},
                    {"InstallationDateJalali", "تاریخ نصب آب"},
                    {"DistanceOfRequestAndInstallation", "زمان تا نصب"},
                    {"AverageDistance", "میانگین فاصله"},
                    {"AverageDistanceNumber", "میانگین فاصله"},
                    {"MaxDistance", "بیشینه فاصله"},
                    {"MinDistance", "کمینه فاصله"},
                    {"DistanceAverage", "میانگین فاصله"},
                    {"DistanceAverageText", "میانگین فاصله"},
                    {"DistanceMedian", "میانه"},
                    {"BlockCode", "کد بلوک"},
                    {"MeterSerial", "سریال کنتور"},
                    {"SiphonType", "نوع سیفون"},
                    {"BodySerial", "سریال کنتور"},
                    {"TotalDebtAmount", "جمع بدهکاری"},
                    {"ChangeCauseTitle", "علت"},
                    {"MeterChangeDate", "تاریخ تعویض"},
                    {"WaterRegistrationDate", "تاریخ ثبت"},
                    {"WaterRequestDate", "تاریخ واگذاری"},
                    {"NatoinalCode", "کد ملی"},
                    {"Text", "متن"},
                    {"SendTime", "زمان ارسال"},
                    {"SendDate", "تاریخ ارسال"},
                    {"FinalDeliveryStateTitle", "وضعیت ارسال"},
                    {"Receiver", "گیرنده"},
                    {"FromValue", "از ضریب"},
                    {"ToValue", "تا ضریب"},
                    {"BillUnitCounts", "آحاد قبض"},
                    {"Item1", "آب بها"},
                    {"Item2", "کارمزد دفع فاضلاب"},
                    {"Item3", "آبونمان آب"},
                    {"Item4", "آبونمان فاضلاب"},
                    {"Item5", "عوارض شهرداری"},
                    {"Item6", "تبصره 2"},
                    {"Item7", "تبصره 2 و 3 قدیم (قابل جمع با آب بها)"},
                    {"Item8", "جریمه"},
                    {"Item9", "آبرسانی"},
                    {"Item10", "ضریب D/ جوانی جمعیت"},
                    {"Item11", "فصل گرم سال "},
                    {"Item12", "ضریب تعدیل"},
                    {"Item13", "تبصره3 آب"},
                    {"Item14", "تبصره3 فاضلاب"},
                    {"Item15", "تبصره آبونمان فاضلاب"},
                    {"Item16", "مبلغ قانون بودجه"},
                    {"Item17", "قسط لوازم کاهنده مصرف"},
                    {"Item18", "بودجه"},
                    {"SumBillCount", "جمع تعداد مشترک"},
                    {"SumBillUnitCounts", "جمع آحاد قبض"},
                    {"SumItem1", "آب بها"},
                    {"SumItem2", "کارمزد دفع فاضلاب"},
                    {"SumItem3", "آبونمان آب"},
                    {"SumItem4", "آبونمان فاضلاب"},
                    {"SumItem5", "عوارض شهرداری"},
                    {"SumItem6", "تبصره 2"},
                    {"SumItem7", "تبصره 2 و 3 قدیم (قابل جمع با آب بها)"},
                    {"SumItem8", "جریمه"},
                    {"SumItem9", "آبرسانی"},
                    {"SumItem10", "ضریب D/ جوانی جمعیت"},
                    {"SumItem11", "فصل گرم سال"},
                    {"SumItem12", "ضریب تعدیل"},
                    {"SumItem13", "تبصره3 آب"},
                    {"SumItem14", "تبصره3 فاضلاب"},
                    {"SumItem15", "تبصره آبونمان فاضلاب"},
                    {"SumItem16", "مبلغ قانون بودجه"},
                    {"SumItem17", "قسط لوازم کاهنده مصرف"},
                    {"SumItem18", "بودجه"},
                    {"LastCounterStateCode", "وضعیت قبلی"},
                    {"LastCounterStateTitle", "وضعیت قبلی"},
                    {"NonDomesticUnit", "آحاد غیر مسکونی"},
                    {"InvoiceAmount", "مبلغ"},
                    {"FromReadingDateJalali", "از تاریخ قبلی قرائت"},
                    {"ToReadingDateJalali", "تا تاریخ قبلی قرائت"},
                    {"RegisterBillDateJalali", "تاریخ صدور قبض"},
                    {"CounterStateId", "وضعیت فعلی"},
                    {"MeterDiameterId", "کد قطر کنتور"},
                    {"IsSelfClaimed", "تسویه"},
                    {"NextDay", "تاریخ فعلی"},
                    {"ReadingCount", "قرائت"},
                    {"CloseCount", "بسته"},
                    {"ObstacleCount", "مانع"},
                    {"ReplacementBranchCount", "کنتور تعویضی "},
                    {"MalfunctionCount", "خراب"},
                    {"AdvancePaymentCount", "برآوردی"},
                    {"PureCount", "تعداد خالص"},
                    {"SelfClaimedCount", "تسویه"},
                    {"Temporarily", "جمع برآوردی"},
                    {"AllCount", "جمع کل"},
                    {"Ruined", "خراب"},
                    {"SumDebt", "جمع بدهکار"},
                    {"SumPureReading", "جمع قرائت"},
                    {"SumClosed", "جمع بسته"},
                    {"SumObstacle", "جمع مانع"},
                    {"SumTemporarily", "جمع برآوردی "},
                    {"SumAll", "جمع کل"},
                    {"SumRuined", "جمع خراب"},
                    {"SumSelfClaimed", "جمع تسویه"},
                    {"Debt", "بدهکار"},
                    {"AverageDuration", "میانگین مدت"},
                    {"MaxDuration", "حداکثر مدت"},
                    {"MinDuration", "حداقل مدت"},
                    {"LatestMalfunctionDateJalali", "تاریخ آخرین وضعیت"},
                    {"CounterStateCode", "وضعیت فعلی"},
                    {"TotalSumItems", "جمع کل"},
                    {"Payble", "مبلغ قابل پرداخت"},
                    {"ItemTitles", "عنوان"},
                    {"PreviousDay", "تاریخ قبلی"},
                    {"CurrentDay", "تاریخ فعلی"},
                    {"CurrentNumber", "شماره کنتور فعلی"},
                    {"GroupKey", "دسته"},
                    {"FromConsumption", "از مصرف"},
                    {"ToConsumption", "تا مصرف"},
                    {"SumConsumptionAverage", "جمع میانگین مصرف"},
                    {"Closed", "بسته"},
                    {"Barrier", "مانع"},
                    {"SumPayable", "مبلغ قابل پرداخت"},
                    {"LatestBillDateJalali", "تاریخ آخرین قبض"},
                    {"LatestReadingDateJalali", "تاریخ آخرین قرائت"}
            };
        }
    }
}
