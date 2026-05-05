namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs
{
    public record ConnectDisconnectPrintInputDto
    {
        public string BillId { get; set; }
        public string CauseId { get; set; }
    }
}
