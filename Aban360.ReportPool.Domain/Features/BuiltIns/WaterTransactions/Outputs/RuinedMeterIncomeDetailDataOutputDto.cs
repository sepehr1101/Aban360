namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record RuinedMeterIncomeDetailDataOutputDto
    {
        public string BillId { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public string CustomerNumber { get; set; }
        public string ReadingNumber { get; set; }
        public int Duration { get; set; }
        public string BranchType { get; set; }
        public string LastReadingDay { get; set; }
        public long Payable { get; set; }
        public long SumItems { get; set; }
        public int Consumption { get; set; }
        public float ConsumptionAverage { get; set; }
        public int CounterStateCode { get; set; }
        public string CounterStateTitle { get; set; }
        public string WaterDiameterTitle { get; set; }
        public string MeterDiameterTitle { get; set; }
        public string UsageTitle { get; set; }
        public int DomesticUnit { get; set; }
        public int CommercialUnit { get; set; }
        public int OtherUnit { get; set; }
        public int TotalUnit { get; set; }
    }
}