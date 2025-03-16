using Aban360.ClaimPool.Domain.Features.Metering.Base;

namespace Aban360.ClaimPool.Domain.Features.WasteWater.Base;

public class WaterMeterSiphonBase
{
    public int Id { get; set; }

    public int WaterMeterId { get; set; }

    public int SiphonId { get; set; }

    public virtual WaterMeterSiphonBase Siphon { get; set; } = null!;

    public virtual WaterMeterBase WaterMeter { get; set; } = null!;
}
