using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.CalculationPool.Domain.Features.Bill.Entities;

[Table(nameof(ImpactSign))]
public class ImpactSign
{
    public short Id { get; set; }//Todo: short or enum?

    public string Title { get; set; } = null!;

    public string? Description { get; set; } 

}
