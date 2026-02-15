namespace Aban360.Common.Literals
{
    public static class SmsTemplates
    {        
        public static string ServiceLinkDisconnectAlert
        {
            get
            {
                return @"آبفا {0}{4}مشترک گرامی به شناسه قبض {1}{4}" +
                         @"باتوجه به عدم توجه به اخطارهای شرکت به علت {2} دستور قطع انشعاب شما صادر گردید. " +
                         @"لذا ظرف مدت {3} ساعت آینده انشعاب شما قطع خواهد شد.";
            }
        }
        public static string ServiceLinkDisconnected
        {
            get
            {
                return @"آبفا {0}{3}مشترک گرامی به شناسه قبض {1}{3}" +
                        @"انشعاب شما در تاریخ {2} قطع گردید." +
                        @"لذا جهت برقراری و وصل مجدد انشعاب آب در ساعات اداری به امور آب و فاضلاب {0} مراجعه نمایید.";
            }
        }
        public static string ServiceLinkConnectAlert
        {
            get
            {
                return @"آبفا {0}{3}مشترک گرامی به شناسه قبض {1}{3}" +
                         @"با عنایت به درخواست جنابعالی مبنی بر وصل مجدد انشعاب، دستور وصل انشعاب شما صادر گردید" +
                         @"لذا ظرف مدت {2} ساعت آینده انشعاب شما وصل خواهد شد."+
                         @"خواهشمند است جهت تحویل انشعاب در محل حضور بعمل آورید";
            }
        }
        public static string ServiceLinkConnected
        {
            get
            {
                return @"آبفا {0}{3}مشترک گرامی به شناسه قبض {1}{3}" +
                        @"انشعاب شما در تاریخ {2} وصل گردید.";
            }
        }

        public static string Bill
        {
            get { return ""; }
        }
    }
}
