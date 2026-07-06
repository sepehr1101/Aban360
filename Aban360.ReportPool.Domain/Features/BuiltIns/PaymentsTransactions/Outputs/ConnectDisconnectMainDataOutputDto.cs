namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record ConnectDisconnectMainDataOutputDto
    {
        public string ZoneTitle { get; set; }
        public string TypeTitle { get; set; }
        public int Count { get; set; }
    }
}
