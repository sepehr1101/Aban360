using System;
using System.Collections.Generic;

namespace Aban360.ClaimPool.Domain.Features.WasteWater;

public partial class SiphonDiameter
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Siphon> Siphons { get; set; } = new List<Siphon>();
}
