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
        public static async Task<string> ExportToExcelAsync<THeader, TData>(THeader tHeader, IEnumerable<TData> tData, string reportName)
        {
            Validate(tHeader, tData);

            var excelfile = new Dictionary<string, object>();
            int sheetCount =tData.Count() / maxDataCount;


            excelfile[ExceptionLiterals.Header] = new List<Dictionary<string, object>> { TranslateHeader(tHeader) };
            for (int i = 0; i < sheetCount + 1; i++)
            {
                var sheetData = tData.Skip(i * maxDataCount).Take(maxDataCount).ToList();
                excelfile[ExceptionLiterals.Page(i + 1)] = TranslateData(sheetData);
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

        private static List<Dictionary<string, object>> TranslateData<T>(IEnumerable<T> data)
        {
            var result = new List<Dictionary<string, object>>();

            var enumerator = data.GetEnumerator();
            if (!enumerator.MoveNext()) return result;

            var first = enumerator.Current;
            var props = GetOrderedProperties(first);
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
            };

        }
    }
}
