using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class ReadingPeriodType
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public short Days { get; set; }

    public short ClientOrder { get; set; }

    public bool IsEnabled { get; set; }

    public short HeadquartersId { get; set; }

    public string HeadquartersTitle { get; set; } = null!;

    public virtual ICollection<ReadingPeriod> ReadingPeriods { get; set; } = new List<ReadingPeriod>();
}
