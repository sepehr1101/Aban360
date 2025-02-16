using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class Region
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public short HeadquartersId { get; set; }

    public virtual Headquarter Headquarters { get; set; } = null!;

    public virtual ICollection<Zone> Zones { get; set; } = new List<Zone>();
}
