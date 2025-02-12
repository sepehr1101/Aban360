using System;
using System.Collections.Generic;

namespace Aban360.ClaimPool.Domain.Features.Metering;

public partial class MeterProducer
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<WaterMeter> WaterMeters { get; set; } = new List<WaterMeter>();
}
