namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs
{
    public record MeterLifeInputDto
    {
        public ICollection<int> ZoneIds { get; set; }
        public ICollection<int> UsageIds { get; set; }

        public int? FromLifeInDay { get; set; }
        public int? ToLifeInDay { get; set; }
    }
}
