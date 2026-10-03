namespace Aban360.ClaimPool.Domain.Features.Tracking.Dto
{
    public record TrackingDetailGetDto
    {
        public int ZoneId { get; set; }
        public Guid TrackId { get; set; }
        public int TrackNumber { get; set; }
        public TrackingDetailGetDto(int zoneId, Guid trackId, int trackNumber)
        {
            ZoneId = zoneId;
            TrackId = trackId;
            TrackNumber = trackNumber;
        }
    }
}