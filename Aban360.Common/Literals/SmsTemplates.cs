using System.Text;

namespace Aban360.Common.Literals
{
    public static class SmsTemplates
    {
        public static string ServiceLinkDisconnectAlert
        {
            get
            {
                return @"آبفا منطقه {0}{7}" +
                        @"مشترک گرامی آقا/خانم {1} به شناسه قبض{2}{7}" +
                         @"با عنایت به عدم توجه به اخطارهای شرکت به علت {3} دستور قطع انشعاب شما صادر گردید.{7} " +
                         @"لذا ظرف مدت {4} ساعت آینده مامور قطع‌و‌وصل آقای {5} به شماره موبایل {6} جهت قطع انشعاب به محل شما مراجعه می‌نماید.";
            }
        }
        public static string ServiceLinkDisconnected
        {
            get
            {
                return @"آبفا منطقه {0}{5}" +
                        @"مشترک گرامی آقا/خانم {1} به شناسه قبض{2}{5}" +
                        @"انشعاب شما در تاریخ {3} توسط مامور قطع‌و‌وصل آقای {4} قطع گردید.{5}" +
                        @" جهت برقراری و وصل مجدد انشعاب آب در ساعات اداری به شرکت آب و فاضلاب منطقه {0} مراجعه نمایید.";
            }
        }
        public static string ServiceLinkConnectAlert
        {
            get
            {
                return @"آبفا منطقه {0}{6}" +
                        @"مشترک گرامی آقا/خانم {1} به شناسه قبض{2}{6}" +
                         @"با عنایت به درخواست جنابعالی مبنی بر وصل مجدد انشعاب، دستور وصل انشعاب شما صادر گردید.{6}" +
                         @"لذا ظرف مدت {3} ساعت آینده مامور قطع‌‌ووصل آقای {4} به شماره موبایل {5} جهت وصل انشعاب به محل شما مراجعه می‌نماید.{6}" +
                         @"خواهشمند است جهت تحویل مجدد انشعاب در محل حضور بعمل آورید";
            }
        }
        public static string ServiceLinkConnected
        {
            get
            {
                return @"آبفا منطقه {0}{4}" +
                        @"مشترک گرامی آقا/خانم {1} به شناسه قبض{2}{4}" +
                        @"انشعاب شما در تاریخ {2} توسط مامور قطع‌ووصل آقای {3} وصل گردید.";
            }
        }
        public static string JudicalNoticeCommandAlert
        {
            get
            {
                return @"باسلام{3} مشترك گرامی جناب آقای / سرکار خانم {0} به شناسه قبض {1}{3}" +
                        @"باتوجه به عدم پرداخت بهای خدمات مصرفی  به مبلغ {2} ریال{3} " +
                        @"به استحضار می رسانددرصورت عدم پرداخت ، طی سه روز آتی،پرونده شما به استنادماده3قانون مجازات استفاده کنندگان غیر مجاز آب وفاضلاب ،جهت طی مراحل مقررات مربوط به اجرای مفاد اسناد رسمی به اجرائیه ثبت ارجاع میگردد.{3}" +
                        @" شرکت آب وفاضلاب استان اصفهان";
            }
        }
        public static string SimpleBill
        {
            get
            {
                string
                    space = " ",
                    abfa = "آبفا",
                    abBahaLiteral = "صورتحساب آب بها" + Environment.NewLine,
                    abBahaAlalHesabLiteral = "علی الحساب آب بها" + Environment.NewLine,
                    fromDateLiteral = "از:",
                    toDateLiteral = "تا:",
                    fromCounterNumberLiteral = "از شماره:",
                    toCounterNumberLiteral = "تا شماره:",
                    thisPeriodAmountLiteral = "دوره:",
                    payableLiteral = "مبلغ:",
                    billIdLiteral = "ش ق:",
                    payIdLiteral = "ش پ:",
                    deadLineLiteral = "مهلت:",
                    rls = "ریال",
                    portalLinkPre = @"https://crm.abfaesfahan.ir/",
                    portalLinkPost = @"/sms/bill",
                    tarefeSaxt = "تعرفه: ساخت و ساز" + Environment.NewLine;
                var sb = new StringBuilder();
                sb.AppendLine(abBahaLiteral + space + "{0}");
                sb.AppendLine(abfa + space + "{1}");
                sb.AppendLine(fromDateLiteral + space + "{2}");
                sb.AppendLine(toDateLiteral + space + "{3}");
                sb.AppendLine(fromCounterNumberLiteral + space + "{4}");
                sb.AppendLine(toCounterNumberLiteral + space + "{5}");
                sb.AppendLine(thisPeriodAmountLiteral + space + "{6}");
                sb.AppendLine(payableLiteral + space + "{7}" + space + rls);
                sb.AppendLine(billIdLiteral + space + "{8}");
                sb.AppendLine(payIdLiteral + space + "{9}");
                sb.AppendLine(deadLineLiteral + space + "{10}");
                sb.AppendLine(portalLinkPre + "{8}" + portalLinkPost);

                return sb.ToString();
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
        public static string RequestRegister
        {
            get
            {
                return @"آبفا استان اصفهان    درخواست شما تایید شد. کد پیگیری:  {0}";
            }
        }
        public static string NewRequestTimeSetAssessment
        {
            get
            {
                return @" ارزیاب محترم آقای {0} ارزیابی جدید شما در تاریخ {1} به"+
                    @"آدرس {2} به نام {3} میباشد شناسه همسایه:{4} تلفن همراه مشترک:{5} "+
                    @"فهرست خدمات: {6} شماره پیگیری:{7} آدرس همسایه: {8}";
            }
        }
        public static string AfterSaleRequestTimeSetAssessment
        {
            get
            {
                return @"ارزیاب محترم {0} ارزیابی جدید شما در تاریخ {1} به آدرس {2} به نام {3} میباشد شناسه مشترک:{4} تلفن همراه مشترک:{5} فهرست خدمات: {6} شماره پیگیری:{7}";
            }
        }
        public static string RequestTimeSet
        {
            get
            {
                return @"متقاضی گرامی مامور بازدید ملک شما آقای {0} با شماره موبایل {1} میباشد."+
                    @"لطفا در تاریخ {2} با اصل کلیه مدارک اعم از پروانه ساختمان ،سند مالکیت یا قولنامه و"+
                    @"کد پستی در محل حضور داشته باشید شماره پیگیری:{3}";
            }
        }
        public static string TankerWater
        {
            get
            {
                return @"آبفا اصفهان - {0} 
صورتحساب فروش آب {1}
{2} {3}
تاریخ: {4}
حجم: {5}
مبلغ: {6} ریال
ش ق: {7}
ش پ: {8}";
            }
        }
    }
}
