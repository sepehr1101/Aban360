namespace Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries
{
    public record TrackingSeenUpdateDto
    {
        public Guid TrackingId { get; set; }
        public bool Seen { get; set; }
        public TrackingSeenUpdateDto(Guid trackingId, bool seen)
        {
            TrackingId = trackingId;
            Seen = seen;
        }
        public TrackingSeenUpdateDto()
        {
        }
    }
}
