namespace Aban360.ClaimPool.Domain.Features.Tracking.Dto
{
    public record TrackingDetailGetDto
    {
        public int ZoneId { get; set; }
        public int TrackNumber { get; set; }
        public TrackingDetailGetDto(int zoneId,int trackNumber)
        {
            ZoneId = zoneId;
            TrackNumber = trackNumber;
        }
    }
}