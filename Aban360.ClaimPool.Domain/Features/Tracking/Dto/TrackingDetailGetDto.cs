namespace Aban360.ClaimPool.Domain.Features.Tracking.Dto
{
    public record TrackingDetailGetDto
    {
        public int ZoneId { get; set; }
        public Guid TrackId { get; set; }
        public string TrackNumber { get; set; }
        public TrackingDetailGetDto(int zoneId, Guid trackId, string trackNumber)
        {
            ZoneId = zoneId;
            TrackId = trackId;
            TrackNumber = trackNumber;
        }
    }
}