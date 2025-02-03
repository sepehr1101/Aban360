using System;
using System.Collections.Generic;

namespace Aban360.LocationPool.Domain.Features.MainHierarchy;

public partial class Municipality
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public short ZoneId { get; set; }

    public virtual Zone Zone { get; set; } = null!;
}
