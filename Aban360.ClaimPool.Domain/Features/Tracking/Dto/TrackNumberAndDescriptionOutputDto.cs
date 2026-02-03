namespace Aban360.ClaimPool.Domain.Features.Tracking.Dto
{
    public record TrackNumberAndDescriptionOutputDto
    {
        public int TrackNumber { get; set; }
        public string? Description { get; set; }
    }
}