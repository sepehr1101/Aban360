using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.CalculationPool.Domain.Features.Rule.Entities;

[Table(nameof(TariffCalculationMode))]
public class TariffCalculationMode
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;
}
