namespace Aban360.UserPool.Domain.Features.Auth.Entities;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? DefaultClaims { get; set; }

    public string Hash { get; set; } = null!;

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
