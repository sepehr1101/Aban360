using Aban360.WorkflowPool.Domain.Constants;
using Aban360.WorkflowPool.Domain.Features.Design.Entities;

namespace Aban360.WorkflowPool.Domain.Features.Design.Dto.Commands
{
    public record WorkflowCreateDto
    {
        public string Title { get; set; } = null!;
        public string? JsonDefinition { get; set; }
        public short Version { get; set; }
        public WorkflowStatusEnum WorkflowStatusId { get; set; }
        public ICollection<StateCreateDto> states { get; set; }
    }
}
