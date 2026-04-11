namespace Aban360.ReportPool.Domain.Features.Request.Outputs
{
    public record TrackingStepGroupDataOutputDto
    {
        public int RegionId { get; set; }
        public string RegionTitle { get; set; }
        public int ZoneId { get; set; }
        public string? ZoneTitle { get; set; }
        public int StatusId { get; set; }
        public string StatusTitle { get; set; }
        public int Count { get; set; }
    }
}
