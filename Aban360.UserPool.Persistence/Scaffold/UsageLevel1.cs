using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class UsageLevel1
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<UsageLevel2> UsageLevel2s { get; set; } = new List<UsageLevel2>();
}
