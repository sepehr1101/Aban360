namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record MalfunctionMeterDataOutputDto
    {
        public string BillId { get; set; }
        public string CustomerNumber { get; set; }
        public string ReadingNumber { get; set; }
        public string ZoneTitle { get; set; }
        public int Duration { get; set; }
        public string BranchType { get; set; }
        public string LastReadingDay { get; set; }
        public string LatestChangeDateJalali { get; set; }
        public string MeterInstallationDateJalali { get; set; }
        public string MeterLife { get; set; }
        public long Payable { get; set; }
        public long SumItems { get; set; }
        public int Consumption { get; set; }
        public float ConsumptionAverage { get; set; }
    }
}
