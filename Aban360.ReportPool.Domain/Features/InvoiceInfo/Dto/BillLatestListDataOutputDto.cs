namespace Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto
{
    public record BillLatestListDataOutputDto
    {
        public int Id { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
        public string ReadingNumber { get; set; }
        public int RegionId { get; set; }
        public string RegionTitle { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; }
        public int BranchTypeId { get; set; }
        public string BranchTypeTitle { get; set; }
        public int Consumption { get; set; }
        public float ConsumptionAverage { get; set; }
        public int CounterStateCode { get; set; }
        public string CounterStateTitle { get; set; }
        public string RegisterDateJalali { get; set; }
        public string PreviousDateJalali { get; set; }
        public int PreviousNumber { get; set; }
    }
}
