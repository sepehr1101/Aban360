namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record SwapRequestTypeOutputDto
    {
        public int TrackNumber { get; set; }
        public int  ServiceGroupId { get; set; }
        public string ServiceGroupTitle { get; set; }
        public ICollection<int> SelectedServices { get; set; }
        public SwapRequestTypeOutputDto(int trackNumber,int serviceGroupId,string serviceGroupTitle,ICollection<int> selectedServices)
        {
            TrackNumber = trackNumber;
            ServiceGroupId = serviceGroupId;
            ServiceGroupTitle = serviceGroupTitle;
            SelectedServices = selectedServices;
        }
        public SwapRequestTypeOutputDto()
        {
        }
    }
}
