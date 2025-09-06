namespace Aban360.Common.Literals
{
    public static class ExceptionLiterals
    {
        public static string WaterBillMinDate { get { return "1401/01/01"; } }
        public static string Date1403_01_01 { get { return "1403/01/01"; } }
        public static string ArgumentIsNull_1 { get { return "آرگومان ارائه شده NULL است. نام آرگومان"; } }
        public static string EmptyString { get { return "نوع داده رشته ای تهی یا خالی است"; } }
        public static string AppBasePathNotFound_1 { get {return "ریشه پروژه در مسیر {0} پیدا نشد"; } }
        public static string InvalidIp { get { return "Ip وارد شده صحیح نمیباشد: {0}"; } }
        public static string MustEnum => "مقدار وارد شده باید یک Enum باشد";
        public static string GreaterThan0 => "مقدار وارد شده باید بزرگ تر از 0 باشد";
        public static string NotNull => "از پر بودن تمامی فیلد ها اطمینان حاصل کنید";
        public static string NotNullAll => "حداقل یکی از فیلد ها را پر کنید";
        public static string Equal10 => "مقدار وارد شده باید برابر 10 کاراکتر باشد";
        public static string NotLessThan6 => "مقدار وارد شده نباید کمتر از 6 کاراکتر باشد";
        public static string NotMoreThan3 => "مقدار وارد شده نباید بیش از 3 کاراکتر باشد";
        public static string NotMoreThan11 => "مقدار وارد شده نباید بیش از 11 کاراکتر باشد";
        public static string NotMoreThan13 => "مقدار وارد شده نباید بیش از 13 کاراکتر باشد";
        public static string NotMoreThan15 => "مقدار وارد شده نباید بیش از 15 کاراکتر باشد";
        public static string NotMoreThan31 => "مقدار وارد شده نباید بیش از 31 کاراکتر باشد";
        public static string NotMoreThan255 => "مقدار وارد شده نباید بیش از 255 کاراکتر باشد";
        public static string NotMoreThan1023 => "مقدار وارد شده نباید بیش از 1023 کاراکتر باشد";
        public static string PreviousDateIsInvalid => "تاریخ قبلی قرائت ناصحیح است";
        public static string CurrentDateIsInvalid => "تاریخ فعلی قرائت ناصحیح است";
        public static string CurrentDateNotMoreThanPreviousDate => "تاریخ دوره قبلی نمیتوان از تاریخ دوره جاری بزرگ تر باشد";
        public static string CurrentNumberNotMoreThanPreviousNumber => "رقم دوره قبلی نمیتوان از رقم دوره جاری بزرگ تر باشد";
        public static string BillIdNotFound => "شناسه قبض یافت نشد!";
        public static string InvalidDate => "تاریخ ناصحیح";
        public static string HasNotSiphon => "سیفون ندارد";

        public static string ToDateMoreThanDate(string date) => $"تاریخ پایان باید بزرگتر از {date} باشد.";
        public static string FromDateMoreThanDate(string date) => $"تاریخ شروع باید بزرگتر از {date} باشد.";
        public static string ToDateLessThanDate(string date) => $"تاریخ پایان باید کوچکتر از {date} باشد.";
        public static string ToDateMoreThanFromDate => $"تاریخ پایان باید بزرگتر از تاریخ شروع باشد.";

        public static string NotFoundPhoneNumber => $"شماره تماس این شناسه یافت نشد";
        public static string NotFoundAnyData => $"اطلاعاتی یافت نشد";
        public static string InvalidRequestData => $"کد درخواست ناصحیح است";
        public static string UnconfirmedRequest => $"درخواست ثبت قطعی نشده است";
        public static string NotCalculation => $"محاسبه انجام نشده است";
        public static string SuccessedPay => $"پرداخت شد";
        public static string UnsuccessedPay => $"پرداخت نشد";

        public static string InvalidPercent => $"درصد اشتباه وارد شده";

        public static string MessageException(string message) => $"خطا : {message}";


        //Excel Errors
        public static string CantGenarateExcelWithNullData => "اطلاعاتی برای ذخیره در فایل اکسل وجود ندارد";
        public static string Header => "سرآیند";
        public static string Page(int i) => $"صفحه {i}";
        public static string NotFoundFile => "فایل پیدا نشد";
        public static string NotFoundAddress => "آدرس پیدا نشد";

        //Configuration
        public static string InvalidConfiguration(string item, string subItem) => $"تنظیمات راه اندازی نرم افزار اشتباه است. {item} > {subItem}";

        public static string InvalidPreviousDateInvoice(int day) => $"صدور قبل برای کمتر از {day} روز امکان پذیر نمی‌باشد.";
    }
}
