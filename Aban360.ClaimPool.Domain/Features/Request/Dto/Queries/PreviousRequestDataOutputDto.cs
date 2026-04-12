namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record PreviousRequestDataOutputDto
    {
        public string BillId { get; set; }
        public int CustomerNumber { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int RegionId { get; set; }
        public string RegionTitle { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; }
        public string RequestsTitle { get; set; }
        public int TrackNumber { get; set; }
        public string StringTrackNumber { get; set; }
        public string RequestDateJalali { get; set; }
        public string RegisterDateJalali { get; set; }
    }
}
