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
        public int RequestOrigin { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsConsiderd { get; set; }
        public TrackingInsertDuplicateDto(int trackNumber, int statusId, string? description, int userId, int requestOrigin, bool isSuccess, bool? isConsiderd)
        {
            TrackNumber = trackNumber;
            StatusId = statusId;
            Description = description;
            UserId = userId;
            RequestOrigin = requestOrigin;
            IsSuccess = isSuccess;
            IsConsiderd = isConsiderd ?? false;
        }
        public TrackingInsertDuplicateDto()
        {

        }
    }
}
