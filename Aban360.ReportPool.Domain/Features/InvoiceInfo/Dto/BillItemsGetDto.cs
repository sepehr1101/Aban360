namespace Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto
{
    public record BillItemsGetDto
    {
        public int Id { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
        public int RegionId { get; set; }
        public string RegionTitle { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; }
        public int BranchTypeId { get; set; }
        public string BranchTypeTitle { get; set; }

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
