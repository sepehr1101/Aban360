using Aban360.Common.BaseEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.UserPool.Domain.Features.Auth.Entities;

[Table(nameof(UserRole))]
public class UserRole:IHashableEntity
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public int RoleId { get; set; }
    public Guid InsertGroupId { get; set; }
    public Guid? RemoveGroupId { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public string InsertLogInfo { get; set; } = null!;
    public string? RemoveLogInfo { get; set; }

    public virtual Role Role { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
