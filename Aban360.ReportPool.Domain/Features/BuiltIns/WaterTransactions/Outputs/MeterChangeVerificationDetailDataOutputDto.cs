namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record MeterChangeVerificationDetailDataOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
        public string ReadingNumber { get; set; }
        public string ChangeDateJalali { get; set; }
        public string ChangeRegisterDateJalali { get; set; }
        public int CounterStateCode { get; set; }
        public string CounterStateTitle { get; set; }
        public string BillRegisterDay { get; set; }
        public string PreviousDateJalali { get; set; }
        public int PreviousNumber { get; set; }
        public string CurrentDateJalali { get; set; }
        public int CurrentNumber { get; set; }
        public string BranchTypeTitle { get; set; }
        public int BranchTypeId { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; }
        public int Consumption { get; set; }
        public float ConsumptionAverage { get; set; }
        public int Duration { get; set; }

    }
}
