namespace Aban360.Common.Literals
{
    public static class ExceptionLiterals
    {
        public static string ArgumentIsNull_1 { get { return "آرگومان ارائه شده NULL است. نام آرگومان"; } }
        public static string EmptyString { get { return "نوع داده رشته ای تهی یا خالی است"; } }
        public static string AppBasePathNotFound_1 { get {return "ریشه پروژه در مسیر {0} پیدا نشد"; } }
        public static string InvalidIp { get { return "Ip وارد شده صحیح نمیباشد: {0}"; } }
        public static string MustEnum => "مقدار وارد شده باید یک Enum باشد";
        public static string GreaterThan0 => "مقدار وارد شده باید بزرگ تر از 0 باشد";
        public static string NotNUll => "از پر بودن تمامی فیلد ها اطمینان حاصل کنید";
        public static string Equal10 => "مقدار وارد شده باید برابر 10 کاراکتر باشد";
        public static string NotMoreThan3 => "مقدار وارد شده نباید بیش از 3 کاراکتر باشد";
        public static string NotMoreThan11 => "مقدار وارد شده نباید بیش از 11 کاراکتر باشد";
        public static string NotMoreThan15 => "مقدار وارد شده نباید بیش از 15 کاراکتر باشد";
        public static string NotMoreThan31 => "مقدار وارد شده نباید بیش از 31 کاراکتر باشد";
        public static string NotMoreThan255 => "مقدار وارد شده نباید بیش از 255 کاراکتر باشد";
        public static string NotMoreThan1023 => "مقدار وارد شده نباید بیش از 1023 کاراکتر باشد";

        public static string MessageException(string message) => $"خطا : {message}";

    }
}
