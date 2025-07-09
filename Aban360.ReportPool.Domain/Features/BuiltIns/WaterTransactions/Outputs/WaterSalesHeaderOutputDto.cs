namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record WaterSalesHeaderOutputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public string ReportDate { get; set; }
        public int RecordCount { get; set; }
        public long SumPayable { get; set; }
    }
}
