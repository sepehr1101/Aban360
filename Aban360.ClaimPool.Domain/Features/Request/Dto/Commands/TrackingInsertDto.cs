using DNTPersianUtils.Core;

namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record TrackingInsertDto
    {
        public int TrackNumber { get; set; }
        public string CurrentDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string? Description { get; set; }
        public int StatusId { get; set; }
        public TrackingInsertDto(int trackNumber,int statusId,string? description)
        {
            TrackNumber = trackNumber;
            StatusId = statusId;
            Description = description;
        }
    }
}
