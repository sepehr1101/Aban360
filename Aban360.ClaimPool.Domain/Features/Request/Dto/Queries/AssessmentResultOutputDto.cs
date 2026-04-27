namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record AssessmentResultOutputDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsSuccess { get; set; }
    }
}