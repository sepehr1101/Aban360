using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class Guild
{
    public short Id { get; set; }

    public short UsageId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Profession> Professions { get; set; } = new List<Profession>();

    public virtual Usage Usage { get; set; } = null!;
}
