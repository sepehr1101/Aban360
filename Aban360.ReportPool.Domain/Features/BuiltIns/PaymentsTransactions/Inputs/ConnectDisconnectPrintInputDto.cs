namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs
{
    public record ConnectDisconnectPrintInputDto
    {
        public string BillId { get; set; } = default!;
        public int? Why { get; set; }
        public string Who { get; set; } = default!;
        public int When { get; set; }
        public string? Description { get; set; }
    }
}
