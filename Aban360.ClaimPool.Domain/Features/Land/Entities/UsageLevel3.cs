using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Land.Entities;

[Table(nameof(UsageLevel3))]
public class UsageLevel3
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public short UsageLevel2Id { get; set; }

    public virtual UsageLevel2 UsageLevel2 { get; set; } = null!;

    public virtual ICollection<UsageLevel4> UsageLevel4s { get; set; } = new List<UsageLevel4>();
}
