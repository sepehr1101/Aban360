using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class Municipality
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int ZoneId { get; set; }

    public bool IsVillage { get; set; }

    public virtual Zone Zone { get; set; } = null!;
}
