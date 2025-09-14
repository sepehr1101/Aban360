namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record MalfunctionMeterByDurationSummaryByZoneDataOutputDto
    {
        public string RegionTitle { get; set; }
        public string ZoneTitle { get; set; }
        public int CustomerCount { get; set; }
        public int TotalUnit { get; set; }
        public int CommercialUnit { get; set; }
        public int DomesticUnit { get; set; }
        public int OtherUnit { get; set; }
        public int UnSpecified { get; set; }
        public int Field0_5 { get; set; }
        public int Field0_75 { get; set; }
        public int Field1 { get; set; }
        public int Field1_2 { get; set; }
        public int Field1_5 { get; set; }
        public int Field2 { get; set; }
        public int Field3 { get; set; }
        public int Field4 { get; set; }
        public int Field5 { get; set; }
        public int MoreThan6 { get; set; }
    }
}
