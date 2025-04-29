using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class MimetypeCategory
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public string Name { get; set; } = null!;
}
