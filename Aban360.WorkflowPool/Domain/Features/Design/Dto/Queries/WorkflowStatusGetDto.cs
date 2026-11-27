using Aban360.WorkflowPool.Domain.Constants;
using Aban360.WorkflowPool.Domain.Features.Design.Entities;

namespace Aban360.WorkflowPool.Domain.Features.Design.Dto.Queries
{
    public record WorkflowStatusGetDto
    {
        public WorkflowStatusEnum Id { get; set; }
        public string Title { get; set; } = null!;
        public virtual ICollection<Workflow> Workflows { get; set; } = new List<Workflow>();
    }
}
