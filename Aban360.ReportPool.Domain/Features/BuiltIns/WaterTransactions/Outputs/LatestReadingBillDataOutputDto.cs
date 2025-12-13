namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record LatestReadingBillDataOutputDto
    {
        public string ReadingNumber { get; set; }
        public int DeletionStateId { get; set; }
        public string DeletionStateTitle { get; set; }
        public int LatestMeterNumber { get; set; }
        public float LatestConsumptionAverage { get; set; }
        public int CounterStateCode { get; set; }
        public string CounterStateTitle { get; set; }
        public string LatestSuccessfullReadingDateJalali { get; set; }
    }
}
