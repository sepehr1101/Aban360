using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.WasteWater.Entities;

[Table(nameof(WaterMeterSiphon), Schema = TableSchema.Name)]
public class WaterMeterSiphon
{
    public int Id { get; set; }

    public int WaterMeterId { get; set; }

    public int SiphonId { get; set; }

    public virtual Siphon Siphon { get; set; } = null!;

    public virtual WaterMeter WaterMeter { get; set; } = null!;
}
