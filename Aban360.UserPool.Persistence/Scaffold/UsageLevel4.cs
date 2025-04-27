using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class UsageLevel4
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public short UsageLevel3Id { get; set; }

    public virtual UsageLevel3 UsageLevel3 { get; set; } = null!;
}
