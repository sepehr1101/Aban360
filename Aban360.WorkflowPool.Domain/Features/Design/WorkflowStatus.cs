using Aban360.WorkflowPool.Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.WorkflowPool.Domain.Features.Design;

[Table(nameof(WorkflowStatus))]
public class WorkflowStatus
{
    public WorkflowStatusEnum Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Workflow> Workflows { get; set; } = new List<Workflow>();
}
