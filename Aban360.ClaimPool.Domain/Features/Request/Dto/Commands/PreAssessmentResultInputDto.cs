namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record PreAssessmentResultInputDto
    {
        public Guid TrackId { get; set; }
        public int ResultId { get; set; }
        public string? Description { get; set; }
    }
}
