namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record AssessmentResultByPreResultOutputDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsPreResult { get; set; }
    }
}