using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class OfferingUnit
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public string Symbol { get; set; } = null!;

    public virtual ICollection<Offering> Offerings { get; set; } = new List<Offering>();
}
