using Aban360.CalculationPool.Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.CalculationPool.Domain.Features.Rule.Entities;

[Table(nameof(TariffCalculationMode))]
public class TariffCalculationMode
{
    public TariffCalculationModeEnum Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;
}