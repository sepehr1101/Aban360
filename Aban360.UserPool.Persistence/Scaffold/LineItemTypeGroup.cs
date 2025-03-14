using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class LineItemTypeGroup
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public short ImpactSignId { get; set; }

    public string? Description { get; set; }

    public virtual ImpactSign ImpactSign { get; set; } = null!;

    public virtual ICollection<LineItemType> LineItemTypes { get; set; } = new List<LineItemType>();
}
