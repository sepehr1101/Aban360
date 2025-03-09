using Aban360.CalculationPool.Domain.Features.Rule.Entties;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.CalculationPool.Domain.Features.Bill.Entities;

[Table(nameof(LineItemType))]
public class LineItemType
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public short LineItemTypeGroupId { get; set; }

    public virtual LineItemTypeGroup LineItemTypeGroup { get; set; } = null!;

    public virtual ICollection<Tariff> Tariffs { get; set; } = new List<Tariff>();
}
