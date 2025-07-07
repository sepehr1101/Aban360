namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record LinkServiceStatementDataOutputDto
    {
        public long FinalAmount { get; set; }
        public long Amount { get; set; }
        public long OffAmount { get; set; }
        public string  TypeTitle { get; set; }
        public string  ZoneTitle { get; set; }
        public int Count { get; set; }

    }
}