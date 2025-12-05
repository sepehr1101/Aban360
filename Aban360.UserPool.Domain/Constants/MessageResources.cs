namespace Aban360.UserPool.Domain.Constants
{
    public class MessageResources
    {
        public static string SuccessfulProccess { get { return "درخواست با موفقیت پردازش شد"; } }
        public static string CaptchaInvalid { get { return "کپچا نادرست است"; } }
        public static string RefreshTokenIsLessThanToken { get { return "زمان تعیین شده برای انقضای refresh token کمتر از زمان token اصلی در نظر گفته شده است"; } }
        public static string UserNotFound { get { return "کاربر پیدا نشد"; } }
        public static string InvalidConfirmCode { get { return "کد نامعتبر یا زمان آن منقضی شده است"; } }
        public static string UnathorizedResource { get { return "دسترسی غیر مجاز"; } }
        public static string UnauthorizedZone { get { return "دسترسی به یک یا چند ناحیه‌ی انتخابی برای شما وجود ندارد"; } }
        public static string CreateUserSuccess { get { return "افزودن کاربر با موفقیت انجام شد"; } }
        public static string UpdateUserSuccess { get { return "ویرایش کاربر با موفقیت انجام شد"; } }
        public static string InvalidId { get { return "عدم تطابق شناسه های وارد شده"; }  }

        #region Invalid Login Reason
        public static string InvalidUsername = "نام کاربری نادرست";
        public static string InvalidPassword = "پسورد نادرست";
        public static string InvalidVerificationCode = "کد دو مرحله ای نادرست";
        public static string ExpiredVerificationCode = "کد دو مرحله ای منقضی شده";
        public static string LockedUser = "تلاش پس از قفل";
        public static string InactiveUser = "تلاش کاربر غیرفعال";
        #endregion


        #region Logout Reason
        public static string ByUser = "توسط کاربر";
        public static string ByAdmin = "توسط ادمین";
        public static string PasswordChange = "تغیر پسورد";
        public static string EditByAdmin = "ویرایش توسط ادمین";
        public static string ExpiredToken = "انقضای توکن";
        public static string ChangeIpInSession = "تغیر IP در جلسه جاری";
        public static string ChangeClientMeta = "تغییر مشخصات کلاینت";
        public static string ConcurrentLogin = "لاگین همزمان";
        #endregion
    }
}
