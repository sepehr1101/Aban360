namespace Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries
{
    public record TrackingSeenUpdateDto
    {
        public IEnumerable<Guid> BlockIds { get; set; }
        public bool Seen { get; set; }
        public TrackingSeenUpdateDto(IEnumerable<Guid> blockIds, bool seen)
        {
            BlockIds = blockIds;
            Seen = seen;
        }
        public TrackingSeenUpdateDto()
        {
        }
    }
}
