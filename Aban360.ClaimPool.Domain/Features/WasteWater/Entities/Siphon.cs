using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features._Base.Entities;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.WasteWater.Entities;

[Table(nameof(Siphon), Schema = TableSchema.Name)]
public class Siphon: SiphonBase
{
    public int WaterMeterId { get; set; }

    public virtual WaterMeter WaterMeter { get; set; } = null!;

    public virtual ICollection<Siphon> InversePrevious { get; set; } = new List<Siphon>();

    public virtual Siphon? Previous { get; set; }

    public virtual SiphonDiameter SiphonDiameter { get; set; } = null!;

    public virtual SiphonMaterial SiphonMaterial { get; set; } = null!;

    public virtual SiphonType SiphonType { get; set; } = null!;
    public virtual ICollection<WaterMeterSiphon> WaterMeterSiphons { get; set; } = new List<WaterMeterSiphon>();
}
