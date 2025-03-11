using Aban360.CalculationPool.Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.CalculationPool.Domain.Features.Bill.Entities;

[Table(nameof(ImpactSign))]
public class ImpactSign
{
    public ImpactSignEnum Id { get; set; }

    public string Title { get; set; } = null!;

    public short Multiplier { get; set; }

    public string? Description { get; set; } 

}
