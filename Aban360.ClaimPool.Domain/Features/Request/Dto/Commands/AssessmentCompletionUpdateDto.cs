namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record AssessmentCompletionUpdateDto
    {
        public Guid Id { get; set; }
        public string AllInJson { get; set; }
        public AssessmentCompletionUpdateDto(Guid id, string allInJson)
        {
            Id = id;
            AllInJson = allInJson;
        }
        public AssessmentCompletionUpdateDto()
        {
        }
    }
}
