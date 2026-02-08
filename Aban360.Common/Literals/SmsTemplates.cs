namespace Aban360.Common.Literals
{
    public static class SmsTemplates
    {        
        public static string ServiceLinkConnect
        {
            get { return $"مشترک گرامی {Environment.NewLine} انشعاب شما وصل گردید. {Environment.NewLine} آبفا استان اصفهان"; }
        }
        public static string ServiceLinkDisconnect
        {
            get { return $"مشترک گرامی{Environment.NewLine} انشعاب شما به دلیل {0} قطع گردید {Environment.NewLine}آبفا استان اصفهان"; }
        }
        public static string Bill
        {
            get { return ""; }
        }
    }
}
