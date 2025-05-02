using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Metering.Entities;

[Table(nameof(WaterMeterInstallationMethod))]
public class WaterMeterInstallationMethod
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<WaterMeter> WaterMeters { get; set; } = new List<WaterMeter>();
}
