namespace Aban360.ClaimPool.Domain.Features.Tracking.Dto
{
    public record TrackingDetailInputDto
    {
        public int ZoneId { get; set; }
        public Guid TrackId { get; set; }
        public string TrackNumber{ get; set; }
        public int StateId { get; set; }
    }
}