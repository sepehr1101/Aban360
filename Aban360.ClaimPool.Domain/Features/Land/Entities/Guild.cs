using Aban360.ClaimPool.Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Land.Entities;

[Table(nameof(Guild), Schema = TableSchema.Name)]
public class Guild
{
    public short Id { get; set; }
    public short UsageId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public virtual ICollection<Profession> Professions { get; set; } = new List<Profession>();
    public virtual Usage Usage { get; set; } = null!;
}
