namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record LatestReadingBillHeaderOutputDto
    {
        public string BillId { get; set; }
        public string ReportDateJalali { get; set; }
    }
}
