namespace Aban360.WorkflowPool.Domain.Features.Design.Dto.Commands
{
    public record StateCreateDto
    {
        public int Code { get; set; }
        public string Title { get; set; } = null!;
        //public int WorkflowId { get; set; }
    }
}
