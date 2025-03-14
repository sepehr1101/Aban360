using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class ReadingPeriod
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public short ReadingPeriodTypeId { get; set; }

    public short ClientOrder { get; set; }

    public virtual ReadingPeriodType ReadingPeriodType { get; set; } = null!;
}
