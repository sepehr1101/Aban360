namespace Aban360.WorkflowPool.Domain.Features.Design.Dto.Queries
{
    public record StateGetDto
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Title { get; set; } = null!;
        public int WorkflowId { get; set; }
    }
}
