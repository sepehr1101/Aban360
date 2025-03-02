using Aban360.ClaimPool.Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Metering.Entities;

[Table(nameof(WaterMeterTagDefinition), Schema = TableSchema.Name)]
public class WaterMeterTagDefinition
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Color { get; set; }

    public virtual ICollection<WaterMeterTag> WaterMeterTags { get; set; } = new List<WaterMeterTag>();
}
