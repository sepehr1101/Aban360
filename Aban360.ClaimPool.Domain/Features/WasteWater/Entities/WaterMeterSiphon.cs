using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features._Base;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.WasteWater.Entities;

[Table(nameof(WaterMeterSiphon), Schema = TableSchema.Name)]
public class WaterMeterSiphon: WaterMeterSiphonBase
{
    public virtual Siphon Siphon { get; set; } = null!;
    public virtual WaterMeter WaterMeter { get; set; } = null!;
}
