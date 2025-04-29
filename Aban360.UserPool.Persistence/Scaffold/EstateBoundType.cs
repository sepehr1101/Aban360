using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class EstateBoundType
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Estate> Estates { get; set; } = new List<Estate>();

    public virtual ICollection<RequestEstate> RequestEstates { get; set; } = new List<RequestEstate>();
}
