using DNTPersianUtils.Core;

namespace Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto
{
    public record BillHistoryHeaderOutputDto
    {
        public string BillId { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public int RecordCount { get; set; }
        public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string Title { get; set; }

        public long Payable { get; set; }
        public long SumItems { get; set; }

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
        public long Boodje { get; set; }
    }
}
