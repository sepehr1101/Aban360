using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.CalculationPool.Domain.Features.Bill.Entities;

[Table(nameof(LineItemTypeGroup))]
public class LineItemTypeGroup
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public short ImpactSign { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<LineItemType> LineItemTypes { get; set; } = new List<LineItemType>();
}
