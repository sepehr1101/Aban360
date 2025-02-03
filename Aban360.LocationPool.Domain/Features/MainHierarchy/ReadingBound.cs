using System;
using System.Collections.Generic;

namespace Aban360.LocationPool.Domain.Features.MainHierarchy;

public partial class ReadingBound
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public short ZoneId { get; set; }

    public virtual ICollection<ReadingBlock> ReadingBlocks { get; set; } = new List<ReadingBlock>();

    public virtual Zone Zone { get; set; } = null!;
}
