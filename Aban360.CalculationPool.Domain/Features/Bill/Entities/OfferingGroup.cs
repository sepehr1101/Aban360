using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.CalculationPool.Domain.Features.Bill.Entities;

[Table(nameof(OfferingGroup))]
public class OfferingGroup
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Offering> Offerings { get; set; } = new List<Offering>();
}
