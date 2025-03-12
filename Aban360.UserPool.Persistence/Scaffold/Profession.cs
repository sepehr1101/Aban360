using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class Profession
{
    public short Id { get; set; }

    public short GuildId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public virtual Guild Guild { get; set; } = null!;
}
