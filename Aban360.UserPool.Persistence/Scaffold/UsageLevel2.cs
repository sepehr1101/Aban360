using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class UsageLevel2
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public short UsageLevel1Id { get; set; }

    public virtual UsageLevel1 UsageLevel1 { get; set; } = null!;

    public virtual ICollection<UsageLevel3> UsageLevel3s { get; set; } = new List<UsageLevel3>();
}
