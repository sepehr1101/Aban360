using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class Workflow
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? JsonDefinition { get; set; }

    public short Version { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    public string? Description { get; set; }

    public short WorkflowStatusId { get; set; }

    public virtual ICollection<State1> State1s { get; set; } = new List<State1>();

    public virtual WorkflowStatus WorkflowStatus { get; set; } = null!;
}
