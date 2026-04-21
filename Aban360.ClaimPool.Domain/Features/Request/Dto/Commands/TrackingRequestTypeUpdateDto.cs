namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record TrackingRequestTypeUpdateDto
    {
        public int TrackNumber { get; set; }
        public int ServiceGroupId { get; set; }
        public string Description { get; set; }
        public TrackingRequestTypeUpdateDto(int trackNumber,int serviceGroupId,string description)
        {
            TrackNumber = trackNumber;
            ServiceGroupId = serviceGroupId;
            Description = description;
        }
        public TrackingRequestTypeUpdateDto()
        {
        }
    }
}
