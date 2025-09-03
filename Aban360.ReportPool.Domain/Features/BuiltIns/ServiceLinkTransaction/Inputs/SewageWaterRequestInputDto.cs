namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs
{
    public record SewageWaterRequestInputDto
    {
        public bool IsWater { get; set; }

        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public ICollection<int> ZoneIds { get; set; }
        public ICollection<int> UsageIds { get; set; }
    }
}
