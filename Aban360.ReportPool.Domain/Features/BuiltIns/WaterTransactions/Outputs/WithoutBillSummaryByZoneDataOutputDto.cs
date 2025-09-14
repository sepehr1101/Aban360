namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record WithoutBillSummaryByZoneDataOutputDto
    {
        public string RegionTitle { get; set; }
        public string ZoneTitle { get; set; }
        public int CustomerCount { get; set; }
        public int TotalUnit { get; set; }
        public int CommercialUnit { get; set; }
        public int DomesticUnit { get; set; }
        public int OtherUnit { get; set; }
    }
}
