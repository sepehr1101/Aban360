using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Land.Entities;

[Table(nameof(UsageLevel4))]
public class UsageLevel4
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public short UsageLevel3Id { get; set; }

    public virtual UsageLevel3 UsageLevel3 { get; set; } = null!;
}
