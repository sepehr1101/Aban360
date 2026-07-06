namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs
{
    public record ConnectDisconnectDetailInputDto
    {
        public int ZoneId { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
    }
}
