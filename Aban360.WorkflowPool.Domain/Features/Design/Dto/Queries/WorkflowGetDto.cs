using Aban360.WorkflowPool.Domain.Constants;
using Aban360.WorkflowPool.Domain.Features.Design.Entities;

namespace Aban360.WorkflowPool.Domain.Features.Design.Dto.Queries
{
    public record WorkflowGetDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? JsonDefinition { get; set; }
        public string? Description { get; set; }
        public short Version { get; set; }
        public WorkflowStatusEnum WorkflowStatusId { get; set; }
        public ICollection<StateGetDto> states { get; set; }
    }
}
