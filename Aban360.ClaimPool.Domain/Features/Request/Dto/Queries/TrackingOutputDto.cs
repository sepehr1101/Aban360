namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record TrackingOutputDto
    {
        public Guid TrackId { get; set; }
        public int TrackNumber { get; set; }
        public string StringTrackNumber { get; set; } = default!;
        public string? BillId { get; set; }
        public int RegionId { get; set; }
        public string RegionTitle { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public string InsertDateJalali { get; set; }
        public int ServiceGroupId { get; set; }
        public string ServiceGroupTitle { get; set; }
        public int StatusId { get; set; }
        public string StatusTitle { get; set; }
        public int InserrtedBy { get; set; }
        public string? Description { get; set; }
        public string NotificationMobile { get; set; }
        public string NeighbourBillId { get; set; }
        public string? Caller { get; set; }
        public string RequestOrigin { get; set; }
    }
}
