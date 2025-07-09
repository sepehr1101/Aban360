namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record WaterModifiedBillsHeaderOutputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public string ReportDateJalali { get; set; }
        public int RecordCount { get; set; }
        public long SumItems { get; set; }
        public long Payable { get; set; }
    }
}
