using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class Zone
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int RegionId { get; set; }

    public string? UnstandardCode { get; set; }

    public virtual ICollection<Municipality> Municipalities { get; set; } = new List<Municipality>();

    public virtual ICollection<ReadingBound> ReadingBounds { get; set; } = new List<ReadingBound>();

    public virtual Region Region { get; set; } = null!;
}
