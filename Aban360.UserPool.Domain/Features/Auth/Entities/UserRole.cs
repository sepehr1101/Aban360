namespace Aban360.UserPool.Domain.Features.Auth.Entities;

public class UserRole
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public int RoleId { get; set; }
    //todo add from to
    public string Hash { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
