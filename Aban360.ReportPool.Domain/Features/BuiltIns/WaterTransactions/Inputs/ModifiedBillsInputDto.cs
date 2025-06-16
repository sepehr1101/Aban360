namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record ModifiedBillsInputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public bool IsRetured { get; set; }
    }
}
