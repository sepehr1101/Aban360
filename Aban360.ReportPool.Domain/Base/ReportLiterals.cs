namespace Aban360.ReportPool.Domain.Base
{
    public static class ReportLiterals
    {
        public static string Report { get { return "گزارش"; } }
        public static string CustomerSearch { get { return $"{Report} جستجوی مشترک"; } }
        public static string UnconfirmedSubscribers{ get { return $"{Report} مشترکین ثبت قطعی نشده"; } }
        public static string PrepaymentAndCalculation{ get { return $"{Report} ریز محاسبه و پیش پرداخت"; } }
        public static string CalculationDetails{ get { return $"{Report} ریز محاسبه"; } }
        public static string DeductionsAndDiscountsReport{ get { return $"{Report} کسورات و تخفیفات"; } }
        public static string DebtorByDay { get { return $"{Report} روزنامه بدهکاران"; } }
        public static string LinkServiceStatement { get { return $"{Report} صورت وضعیت (سامانه فروش)"; } }
        public static string PendingPayments { get { return $"{Report} مانده مطالبات"; } }
    }
}
