using DNTPersianUtils.Core;

namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record TrackingInsertDuplicateDto
    {
        public Guid TrackId { get; set; } = Guid.NewGuid();
        public int TrackNumber { get; set; }
        public string CurrentDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string? Description { get; set; }
        public int StatusId { get; set; }
        public int UserId { get; set; }
        public TrackingInsertDuplicateDto(int trackNumber, int statusId, string? description, int userId)
        {
            TrackNumber = trackNumber;
            StatusId = statusId;
            Description = description;
            UserId = userId;
        }
        public TrackingInsertDuplicateDto()
        {

        }
    }
}
