using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class MeterMaterial
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<RequestWaterMeter> RequestWaterMeters { get; set; } = new List<RequestWaterMeter>();

    public virtual ICollection<WaterMeter> WaterMeters { get; set; } = new List<WaterMeter>();
}
