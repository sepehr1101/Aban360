namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs
{
    public record FinancialStatementInputDto
    {
        public IEnumerable<int> ZoneIds { get; set; }
        public IEnumerable<int> UsageGroupIds { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
    }
}
