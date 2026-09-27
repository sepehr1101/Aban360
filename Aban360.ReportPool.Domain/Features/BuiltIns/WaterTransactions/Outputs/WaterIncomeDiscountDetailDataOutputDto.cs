namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record WaterIncomeDiscountDetailDataOutputDto
    {
        public string RegionTitle { get; set; }
        public string ZoneTitle { get; set; }
        public string BillId { get; set; }
        public string UsageTitle { get; set; }
        public string ReadingNumber { get; set; }
        public float SewageConsumption { get; set; }
        public int Consumption { get; set; }
        public float ConsumptionAverage { get; set; }
        public string MeterDiameterTitle { get; set; }
        public int BillUnitCounts { get; set; }
        public string UseStateTitle { get; set; }
        public string BranchType { get; set; }
        public int Duration { get; set; }
        public long SumItemsOff { get; set; }
        public long SumOffWater { get; set; }
        public long ItemOff1 { get; set; }
        public long ItemOff2 { get; set; }
        public long ItemOff3 { get; set; }
        public long ItemOff4 { get; set; }
        public long ItemOff5 { get; set; }
        public long ItemOff6 { get; set; }
        public long ItemOff7 { get; set; }
        public long ItemOff8 { get; set; }
        public long ItemOff9 { get; set; }
        public long ItemOff10 { get; set; }
        public long ItemOff11 { get; set; }
        public long ItemOff12 { get; set; }
        public long ItemOff13 { get; set; }
        public long ItemOff14 { get; set; }
        public long ItemOff15 { get; set; }
        public long ItemOff16 { get; set; }
        public long ItemOff17 { get; set; }
        public long ItemOff18 { get; set; }
        public int BillUnit { get; set; }
        public int TotalUnit { get; set; }
    }
}
