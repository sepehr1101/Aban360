using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class LineItemType
{
    public short Id { get; set; }

    public short LineItemTypeGroupId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public virtual LineItemTypeGroup LineItemTypeGroup { get; set; } = null!;

    public virtual ICollection<Tariff> Tariffs { get; set; } = new List<Tariff>();
}
