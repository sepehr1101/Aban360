namespace Aban360.WorkflowPool.Domain.Features.Design.Dto.Commands
{
    public record WorkflowUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? JsonDefinition { get; set; }
        public short Version { get; set; }
        public short WorkflowStatusId { get; set; }
    }
}
