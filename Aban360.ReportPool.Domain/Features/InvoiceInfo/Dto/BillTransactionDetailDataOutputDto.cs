namespace Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto
{
    public record BillTransactionDetailDataOutputDto
    {
        public int UsageSellId { get; set; }
        public string UsageSellTitle { get; set; }
        public int UsageConsumptionId { get; set; }
        public string UsageConsumptionTitle { get; set; }
        public string BranchTypeTitle { get; set; }
        public int BranchTypeId { get; set; }
        public string PreviousDateJalali { get; set; }
        public string CurrentDateJalali { get; set; }
        public string RegisterDateJalali { get; set; }
        public int DomesticUnit { get; set; }
        public int CommertialUnit { get; set; }
        public int OtherUnit { get; set; }
        public int PreviousNumber { get; set; }
        public int NextNumber { get; set; }
        public int Consumption { get; set; }
        public float ConsumptionAverage { get; set; }
        public long SumItems { get; set; }
        public int EmptyCount { get; set; }
        public int CounterStateCode { get; set; }
        public string CounterStateTitle { get; set; }

    }
}
