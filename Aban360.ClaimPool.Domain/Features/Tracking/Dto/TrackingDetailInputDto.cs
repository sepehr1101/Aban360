namespace Aban360.ClaimPool.Domain.Features.Tracking.Dto
{
    public record TrackingDetailInputDto
    {
        public int ZoneId { get; set; }
        public int TrackNumber { get; set; }
        public int StateId { get; set; }
    }
}