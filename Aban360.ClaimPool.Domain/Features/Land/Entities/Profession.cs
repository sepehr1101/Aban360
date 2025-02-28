using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Land.Entities;

[Table(nameof(Profession))]
public class Profession
{
    public short Id { get; set; }

    public short GuildId { get; set; }

    public string Title { get; set; } = null!;

    public short Description { get; set; }

    public virtual Guild Guild { get; set; } = null!;
}
