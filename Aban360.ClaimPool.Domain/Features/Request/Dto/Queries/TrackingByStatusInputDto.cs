namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record TrackingByStatusInputDto
    {
        public IEnumerable<int> ZoneIds { get; set; }
        public int StatusId { get; set; }
        public TrackingByStatusInputDto(IEnumerable<int> zoneIds,int statusId)
        {
            ZoneIds = zoneIds;
            StatusId = statusId;
        }
        public TrackingByStatusInputDto()
        {
        }
    }
}