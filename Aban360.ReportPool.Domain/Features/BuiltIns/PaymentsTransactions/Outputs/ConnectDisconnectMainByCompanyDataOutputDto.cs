namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record ConnectDisconnectMainByCompanyDataOutputDto
    {
        public string ZoneTitle { get; set; }
        public string CompanyTitle { get; set; }
        public string TypeTitle { get; set; }
        public int Count { get; set; }
    }
}
