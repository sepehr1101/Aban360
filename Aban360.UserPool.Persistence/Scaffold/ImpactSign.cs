using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class ImpactSign
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public short Multiplier { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<LineItemTypeGroup> LineItemTypeGroups { get; set; } = new List<LineItemTypeGroup>();
}
