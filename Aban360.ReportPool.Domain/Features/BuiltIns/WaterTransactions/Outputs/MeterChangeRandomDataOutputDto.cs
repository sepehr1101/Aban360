namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record MeterChangeRandomDataOutputDto
    {
        public string ZoneTitle { get; set; }
        public string BillId { get; set; }
        public string FullName { get; set; }
        public string UsageTitle { get; set; }
        public string WaterDiameterTitle { get; set; }
        public int Consumption { get; set; }
        public float ConsumptionAverage { get; set; }
        public int Duration { get; set; }
        public int TotalUnit { get; set; }
        public string PreviousDay { get; set; }
        public string NextDay { get; set; }
        public int PreviousNumber { get; set; }
        public int NextNumber { get; set; }
        public string CounterStateTitle { get; set; }
        public int MeterNumber { get; set; }
        public string RegisterDateJalali { get; set; }
        public string ChangeDateJalali { get; set; }

    }
}