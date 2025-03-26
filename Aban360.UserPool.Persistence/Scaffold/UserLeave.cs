using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class UserLeave
{
    public short Id { get; set; }

    public short RegiatereId { get; set; }

    public string RegiatereFullname { get; set; } = null!;

    public DateTime RegiatereDatetime { get; set; }

    public Guid UserId { get; set; }

    public string UserFullname { get; set; } = null!;

    public string FromDateJalali { get; set; } = null!;

    public string ToDateJalali { get; set; } = null!;
}
