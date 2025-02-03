using System;
using System.Collections.Generic;

namespace Aban360.LocationPool.Domain.Features.MainHierarchy;

public partial class Zone
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public short RegionId { get; set; }

    public string UnstandardCode { get; set; } = null!;

    public virtual ICollection<Municipality> Municipalities { get; set; } = new List<Municipality>();

    public virtual ICollection<ReadingBound> ReadingBounds { get; set; } = new List<ReadingBound>();

    public virtual Region Region { get; set; } = null!;
}
