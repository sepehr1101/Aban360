using Aban360.UserPool.Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.UserPool.Domain.Features.Auth.Entities;

[Table(nameof(InvalidLoginReason))]
public class InvalidLoginReason
{
    public InvalidLoginReasonEnum Id { get; set; }
    
    public string Title { get; set; } = null!;

    public virtual ICollection<UserLogin> UserLogins { get; set; } = new List<UserLogin>();
}
