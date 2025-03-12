using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class TariffConstant
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public string Condition { get; set; } = null!;

    public string Key { get; set; } = null!;

    public string FromDateJalali { get; set; } = null!;

    public string ToDateJalali { get; set; } = null!;

    public string? Description { get; set; }
}
