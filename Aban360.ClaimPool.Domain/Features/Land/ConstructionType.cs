using System;
using System.Collections.Generic;

namespace Aban360.ClaimPool.Domain.Features.Land;

public partial class ConstructionType
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Estate> Estates { get; set; } = new List<Estate>();
}
