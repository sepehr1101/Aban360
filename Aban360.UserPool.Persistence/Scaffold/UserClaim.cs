﻿using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class UserClaim
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public short ClaimTypeId { get; set; }

    public string ClaimValue { get; set; } = null!;

    public Guid InsertGroupId { get; set; }

    public Guid? RemoveGroupId { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    public string InsertLogInfo { get; set; } = null!;

    public string? RemoveLogInfo { get; set; }

    public string Hash { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
