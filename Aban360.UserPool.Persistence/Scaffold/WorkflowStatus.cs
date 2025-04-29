using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class WorkflowStatus
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Workflow> Workflows { get; set; } = new List<Workflow>();
}
