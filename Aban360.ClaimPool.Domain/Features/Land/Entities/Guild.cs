using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Land.Entities;

[Table(nameof(Guild))]
public class Guild
{
    public short Id { get; set; }

    public short UsageId { get; set; }

    public string Title { get; set; } = null!;

    public short Description { get; set; }

    public virtual ICollection<Profession> Professions { get; set; } = new List<Profession>();

    public virtual Usage Usage { get; set; } = null!;
}
