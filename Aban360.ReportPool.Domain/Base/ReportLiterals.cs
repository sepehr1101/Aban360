namespace Aban360.ReportPool.Domain.Base
{
    public static class ReportLiterals
    {
        public static string Report { get { return "گزارش"; } }
        public static string CustomerSearch { get { return $"{Report} جستجوی مشترک"; } }
        public static string UnconfirmedSubscribers{ get { return $"{Report} مشترکین ثبت قطعی نشده"; } }
        public static string PrepaymentAndCalculation{ get { return $"{Report} ریز محاسبه و پیش پرداخت"; } }
    }
}
