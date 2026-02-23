namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record DisplayThisBillInput
    {
        public long Id { get; set; }
        public string? BillId { get; set; }
        public bool DisplayPreviousDebt { get; set; }
    }
}
