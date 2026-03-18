namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record TrackingKartableHeaderOutputDto
    {
        public int ZoneCount { get; set; }
        public int RequestCount { get; set; }
        public string CurrentDateJalali { get; set; }
        public string Title { get; set; }
    }
}
