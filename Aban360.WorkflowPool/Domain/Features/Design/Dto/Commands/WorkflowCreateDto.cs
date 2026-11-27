using Aban360.WorkflowPool.Domain.Constants;
using Aban360.WorkflowPool.Domain.Features.Design.Entities;

namespace Aban360.WorkflowPool.Domain.Features.Design.Dto.Commands
{
    public record WorkflowCreateDto
    {
        public string Title { get; set; } = null!;
        public string? JsonDefinition { get; set; }
        public string? Description { get; set; }
        public ICollection<StateCreateDto> states { get; set; }
    }
}
