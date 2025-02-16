using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class ReadingBound
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string FromReadingNumber { get; set; } = null!;

    public string ToReadingNumber { get; set; } = null!;

    public int ZoneId { get; set; }

    public virtual ICollection<ReadingBlock> ReadingBlocks { get; set; } = new List<ReadingBlock>();

    public virtual Zone Zone { get; set; } = null!;
}
