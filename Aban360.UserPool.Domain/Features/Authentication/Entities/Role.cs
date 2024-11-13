using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Features.Authentication;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Hash { get; set; } = null!;

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
