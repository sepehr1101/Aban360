using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Land.Entities;

[Table(nameof(Usage))]
public partial class Usage
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public short ProvienceId { get; set; }

    public virtual ICollection<Estate> EstateUsageConsumtions { get; set; } = new List<Estate>();

    public virtual ICollection<Estate> EstateUsageSells { get; set; } = new List<Estate>();
}
