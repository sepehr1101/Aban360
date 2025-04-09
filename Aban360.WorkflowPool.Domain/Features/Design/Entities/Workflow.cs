using Aban360.WorkflowPool.Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.WorkflowPool.Domain.Features.Design.Entities;

[Table(nameof(Workflow))]
public class Workflow
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? JsonDefinition { get; set; }

    public string? Description { get; set; }

    public short Version { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    public WorkflowStatusEnum WorkflowStatusId { get; set; }

    public virtual WorkflowStatus WorkflowStatus { get; set; } = null!;
}
