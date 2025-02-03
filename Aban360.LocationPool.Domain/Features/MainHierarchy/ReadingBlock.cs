using System;
using System.Collections.Generic;

namespace Aban360.LocationPool.Domain.Features.MainHierarchy;

public partial class ReadingBlock
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public short ReadingBoundId { get; set; }

    public virtual ReadingBound ReadingBound { get; set; } = null!;
}
