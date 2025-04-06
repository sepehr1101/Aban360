namespace Aban360.WorkflowPool.Domain.Features.Design.Dto.Queries
{
    public record WorkflowGetDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? JsonDefinition { get; set; }
        public short Version { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public short WorkflowStatusId { get; set; }
    }
}
