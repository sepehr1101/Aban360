namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record WaterDiscountSummaryDataOutputDto
    {
        public int ItemId { get; set; }
        public string ItemTitle { get; set; }
        public int CommercialUnit { get; set; }
        public int DomesticUnit { get; set; }
        public int OtherUnit { get; set; }
        public int TotalUnit { get; set; }
        public int BillUnit { get; set; }
        public int Consumption { get; set; }

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
