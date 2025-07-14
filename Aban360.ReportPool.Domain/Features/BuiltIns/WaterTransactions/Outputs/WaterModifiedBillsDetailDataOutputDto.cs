namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record WaterModifiedBillsDetailDataOutputDto
    {
        public string ZoneTitle { get; set; }
        public string CustomerNumber { get; set; }
        public string UsageSellTitle { get; set; }
        public string RegisterDateJalali { get; set; }
        public long Payable { get; set; }
        public long SumItems { get; set; }

    }
}
