using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class SiphonDiameter
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<RequestSiphon> RequestSiphons { get; set; } = new List<RequestSiphon>();

    public virtual ICollection<Siphon> Siphons { get; set; } = new List<Siphon>();
}
