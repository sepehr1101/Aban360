using Aban360.ClaimPool.Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Metering.Entities;

[Table(nameof(MeterMaterial), Schema = TableSchema.Name)]
public class MeterMaterial
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<WaterMeter> WaterMeters { get; set; } = new List<WaterMeter>();
}
