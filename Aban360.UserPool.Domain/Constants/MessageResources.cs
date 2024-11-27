namespace Aban360.UserPool.Domain.Constants
{
    public class MessageResources
    {
        public static string SuccessfulProccess { get { return "درخواست با موفقیت پردازش شد"; } }
        public static string CaptchaInvalid { get { return "کپچا نادرست است"; } }
        public static string RefreshTokenIsLessThanToken { get { return "زمان تعیین شده برای انقضای refresh token کمتر از زمان token اصلی در نظر گفته شده است"; } }

    }
}
