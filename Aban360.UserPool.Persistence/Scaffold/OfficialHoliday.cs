using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class OfficialHoliday
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public string DateJalali { get; set; } = null!;
}
