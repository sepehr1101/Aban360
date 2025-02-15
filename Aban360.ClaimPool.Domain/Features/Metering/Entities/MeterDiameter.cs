using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Metering.Entities;

[Table(nameof(MeterDiameter))]
public class MeterDiameter
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<WaterMeter> WaterMeters { get; set; } = new List<WaterMeter>();
}
