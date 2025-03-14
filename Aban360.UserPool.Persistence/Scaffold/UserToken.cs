﻿using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class UserToken
{
    public long Id { get; set; }

    public Guid UserId { get; set; }

    public DateTime AccessTokenExpiresDateTime { get; set; }

    public string AccessTokenHash { get; set; } = null!;

    public DateTime RefreshTokenExpiresDateTime { get; set; }

    public string RefreshTokenIdHash { get; set; } = null!;

    public string? RefreshTokenIdHashSource { get; set; }

    public virtual User User { get; set; } = null!;
}
