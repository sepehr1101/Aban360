using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class Headquarter
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public short ProvinceId { get; set; }

    public virtual Province Province { get; set; } = null!;

    public virtual ICollection<Region> Regions { get; set; } = new List<Region>();
}
