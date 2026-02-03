namespace Aban360.ClaimPool.Domain.Features.Tracking.Dto
{
    public record TrackingDisplayFlowDateOutputDto
    {
        public int ZoneId { get; set; }
        public int StatusId { get; set; }
        public string StatusTitle { get; set; }
        public string RegisterDateJalali { get; set; }
        public string RegisterTime { get; set; }
        public string UserDisplayName { get; set; }
        public Guid TrackingId { get; set; }
        public bool HasDetails { get; set; }
        public bool HasSms { get; set; }
        public string? Description { get; set; }
    }
}
