namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record TrackingKartableDataOutputDto
    {
        public Guid TrackId { get; set; }
        public int TrackNumber { get; set; }
        public string? BillId { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int ServiceGroupId { get; set; }
        public string ServiceGroupTitle { get; set; }
        public int StatusId { get; set; }
        public string StatusTitle { get; set; }
        public string NeighbourBillId { get; set; }
        public bool HasAttention { get; set; }
        public string RequestDateJalali { get; set; }
    }
}
