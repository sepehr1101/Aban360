using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class UserWorkday
{
    public short Id { get; set; }

    public Guid UserId { get; set; }

    public string UserFullname { get; set; } = null!;

    public string FromReadingNumber { get; set; } = null!;

    public string ToReadingNumber { get; set; } = null!;

    public string DateJalali { get; set; } = null!;

    public int ZoneId { get; set; }

    public string ZoneTitle { get; set; } = null!;
}
