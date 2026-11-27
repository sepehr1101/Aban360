namespace Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries
{
    public record TrackingListSeenUpdateDto
    {
        public IEnumerable<Guid> BlockIds { get; set; }
        public bool Seen { get; set; }
        public TrackingListSeenUpdateDto(IEnumerable<Guid> blockIds, bool seen)
        {
            BlockIds = blockIds;
            Seen = seen;
        }
        public TrackingListSeenUpdateDto()
        {
        }
    }
}
