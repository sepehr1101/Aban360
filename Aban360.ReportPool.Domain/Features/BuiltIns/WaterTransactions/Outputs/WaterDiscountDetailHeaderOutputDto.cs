using DNTPersianUtils.Core;

namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record WaterDiscountDetailHeaderOutputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public int RecordCount { get; set; }
        public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string Title { get; set; }
        public int CustomerCount { get; set; }
        public int BillCount { get; set; }

        public long AbBaha { get; set; }
        public long FazelabBaha { get; set; }
        public long AbonmanAb { get; set; }
        public long AbonmanFazelab { get; set; }
        public long Maliat { get; set; }
        public long Tabsare2 { get; set; }
        public long Tabsare2_3 { get; set; }
        public long Jarime { get; set; }
        public long Abresani { get; set; }
        public long JavaniJamiat { get; set; }
        public long FaslGarm { get; set; }
        public long ZaribTadil { get; set; }
        public long Tabsare3Ab { get; set; }
        public long Tabsare3Fazelab { get; set; }
        public long TabsareAbonmanFazelab { get; set; }
        public long GhanonBoodje { get; set; }
        public long JavazemKahande { get; set; }
        public long AvarezSanati { get; set; }

    }
}
