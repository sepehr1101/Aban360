using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Land.Entities;

[Table(nameof(CapacityCalculationIndex))]
public class CapacityCalculationIndex
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; } 

    public virtual ICollection<Estate> Estates { get; set; } = new List<Estate>();

}
