namespace Aban360.ReportPool.Domain.Features.Request.Inputs
{
    public record TrackingInputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public ICollection<int> ZoneIds { get; set; }
        public bool IsZoneGroup { get; set; }
    }
}
