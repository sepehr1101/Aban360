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
        public static string SimpleBill
        {
            get
            {
                return "قبض جدید آب بها برای شما صادر گردید";
            }
        }
        //public static string GenerateBillSms()
        //{
        //    string
        //        abBahaLiteral = "صورتحساب آب بها" + Environment.NewLine,
        //        abBahaAlalHesabLiteral = "علی الحساب آب بها" + Environment.NewLine,
        //        fromDateLiteral = "از: ",
        //        toDateLiteral = "تا: ",
        //        fromCounterNumberLiteral = "از شماره: ",
        //        toCounterNumberLiteral = "تا شماره: ",
        //        thisPeriodAmountLiteral = "دوره: ",
        //        payableLiteral = "مبلغ: ",
        //        billIdLiteral = "ش ق: ",
        //        payIdLiteral = "ش پ: ",
        //        deadLineLiteral = "مهلت: ",
        //        spaceRialsLiteral = " ریال",
        //        tarefeSaxt = "تعرفه: ساخت و ساز" + Environment.NewLine;

        //    var preBedBes = (bedBesMember.BedBes.jam + bedBesMember.BedBes.Taxfif) - (bedBesMember.BedBes.baha /*+ bedBesMember.BedBes.kasr_ha*/);
        //    var preBedBesLiteral = preBedBes < 0 ? "بستانکاری: " : "بدهی: ";
        //    var nameValue = bedBesMember.Member.FirstName.Trim() + " " + bedBesMember.Member.SureName.Trim();
        //    var fromDateValue = bedBesMember.BedBes.FromQeraatDate;
        //    var toDateValue = bedBesMember.BedBes.ToQeraatDate;
        //    var fromNumberValue = bedBesMember.BedBes.FromQeraatNumber;
        //    var toNumberValue = bedBesMember.BedBes.ToQeraatNumber;
        //    var thisPeriodAmountValue = bedBesMember.BedBes.baha + bedBesMember.BedBes.kasr_ha;
        //    var payableValue = bedBesMember.BedBes.Pard;
        //    var preBedBesValue = Math.Abs(preBedBes);
        //    var billIdValue = bedBesMember.Member.BillId.Trim();
        //    var payIdValue = bedBesMember.BedBes.PayId.Trim();
        //    var deadlineValue = bedBesMember.BedBes.jam - bedBesMember.BedBes.baha > 10000 ? "فوری" : mohlat;
        //    var linkValue = @"https://crm.abfaesfahan.ir/" + billIdValue + "/sms/bill";

        //    var hasBillPayDead = bedBesMember.BedBes.Pard > 0 ? true : false;
        //    var tarefeSaxtValue = bedBesMember.Member.NoeVagozari == 4 ? tarefeSaxt : "";

        //    var message = string.Concat(
        //             zoneTitle, Environment.NewLine,
        //             bedBesMember.BedBes.CounterStateId == 8 ? abBahaAlalHesabLiteral : abBahaLiteral,
        //             nameValue, Environment.NewLine,
        //             fromDateLiteral, fromDateValue, Environment.NewLine,
        //             toDateLiteral, toDateValue, Environment.NewLine,
        //             fromCounterNumberLiteral, fromNumberValue, Environment.NewLine,
        //             toCounterNumberLiteral, toNumberValue, Environment.NewLine,
        //             tarefeSaxtValue,
        //             thisPeriodAmountLiteral, thisPeriodAmountValue, spaceRialsLiteral, Environment.NewLine,
        //             preBedBesLiteral, preBedBesValue, spaceRialsLiteral, Environment.NewLine,
        //             payableLiteral, payableValue, spaceRialsLiteral, Environment.NewLine,
        //             billIdLiteral, billIdValue, Environment.NewLine,
        //             hasBillPayDead ? payIdLiteral : string.Empty, hasBillPayDead ? payIdValue : string.Empty, hasBillPayDead ? Environment.NewLine : string.Empty,
        //             hasBillPayDead ? deadLineLiteral : string.Empty, hasBillPayDead ? deadlineValue : string.Empty, Environment.NewLine,
        //             linkValue);
        //    return message;
        //}

        public static string Bill
        {
            get { return ""; }
        }
    }
}
