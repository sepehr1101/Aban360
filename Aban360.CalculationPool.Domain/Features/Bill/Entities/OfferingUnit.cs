using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.CalculationPool.Domain.Features.Bill.Entities;

[Table(nameof(OfferingUnit))]
public class OfferingUnit
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public string Symbol { get; set; } = null!;

    public virtual ICollection<Offering> Offerings { get; set; } = new List<Offering>();
}
