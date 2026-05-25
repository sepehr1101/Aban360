namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record WaterModifiedBillsDetailDataOutputDto
    {
        public string ZoneTitle { get; set; }
        public string CustomerNumber { get; set; }
        public string BillId { get; set; }
        public string MeterDiameterId { get; set; }
        public string MeterDiameterTitle { get; set; }
        public string UsageSellTitle { get; set; }
        public string RegisterDateJalali { get; set; }
        public long Payable { get; set; }
        public long SumItems { get; set; }
        public int ReturnCauseId { get; set; }
        public string? ReturnCauseTitle { get; set; }

    }
}
