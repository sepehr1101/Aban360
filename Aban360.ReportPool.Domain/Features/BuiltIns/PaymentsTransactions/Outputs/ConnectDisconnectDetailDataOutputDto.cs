namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record ConnectDisconnectDetailDataOutputDto
    {
        public string ZoneTitle { get; set; }
        public string TypeTitle { get; set; }
        public string CommandCauseTitle { get; set; }
        public string ResultTitle { get; set; }
        public int Count { get; set; }
    }
}
