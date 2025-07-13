namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record ReadingListHeaderOutputDto
    {
        public string ReportDateJalali { get; set; }
        public int RecordCount { get; set; }
    }
}
