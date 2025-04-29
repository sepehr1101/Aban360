using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class UsageLevel3
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public short UsageLevel2Id { get; set; }

    public virtual UsageLevel2 UsageLevel2 { get; set; } = null!;

    public virtual ICollection<UsageLevel4> UsageLevel4s { get; set; } = new List<UsageLevel4>();
}
