namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record LightAssessmentResultInputDto
    {
        public int TrackNumber { get; set; }
        public int ResultId { get; set; }
        public string? Description { get; set; }
    }
}
