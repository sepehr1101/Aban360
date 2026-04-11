using DNTPersianUtils.Core;

namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record TrackingInsertDto
    {
        public Guid TrackId { get; set; } = Guid.NewGuid();
        public int TrackNumber { get; set; }
        public int ZoneId { get; set; }
        public DateTime CurrentDateGregorian { get; set; } = DateTime.Now;
        public string CurrentDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string? BillId { get; set; }
        public int ServiceGroupId { get; set; }
        public int StatusId { get; set; }
        public int InsertByUserId { get; set; }
        public string? Description { get; set; }
        public string NotificationMobile { get; set; }
        public string? NeighbourBillId { get; set; }
        public int RequestOrigin { get; set; }

    }
}
