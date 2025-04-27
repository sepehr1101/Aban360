using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class State1
{
    public int Id { get; set; }

    public int Code { get; set; }

    public string Title { get; set; } = null!;

    public int WorkflowId { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    public string InsertLogInfo { get; set; } = null!;

    public string? RemoveLogInfo { get; set; }

    public string Hash { get; set; } = null!;

    public virtual Workflow Workflow { get; set; } = null!;
}
