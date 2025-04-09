using Aban360.WorkflowPool.Domain.Constants;

namespace Aban360.WorkflowPool.Domain.Features.Design.Dto.Commands
{
    public record WorkflowUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? JsonDefinition { get; set; }
        public string? Description { get; set; }
        public short Version { get; set; }
        public WorkflowStatusEnum WorkflowStatusId { get; set; }
    }
}
