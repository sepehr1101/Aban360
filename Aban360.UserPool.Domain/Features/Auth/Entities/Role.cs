using Aban360.Common.BaseEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.UserPool.Domain.Features.Auth.Entities;

[Table(nameof(Role))]
public class Role: IHashableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? DefaultClaims { get; set; }
    public bool SensitiveInfo { get; set; }
    public bool IsRemovable { get; set; }
    public int? PreviousId { get; set; }

    public virtual ICollection<Role> InversePrevious { get; set; } = new List<Role>();
    public virtual Role? Previous { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
