using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class ReadingBlock
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public string FromReadingNumber { get; set; } = null!;

    public string ToReadingNumber { get; set; } = null!;

    public int ReadingBoundId { get; set; }

    public virtual ReadingBound ReadingBound { get; set; } = null!;
}
