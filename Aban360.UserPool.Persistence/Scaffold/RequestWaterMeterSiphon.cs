using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class RequestWaterMeterSiphon
{
    public int Id { get; set; }

    public int WaterMeterId { get; set; }

    public int SiphonId { get; set; }

    public virtual RequestSiphon Siphon { get; set; } = null!;

    public virtual RequestWaterMeter WaterMeter { get; set; } = null!;
}
