using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class WaterMeterSiphon
{
    public int Id { get; set; }

    public int WaterMeterId { get; set; }

    public int SiphonId { get; set; }

    public virtual Siphon Siphon { get; set; } = null!;

    public virtual WaterMeter WaterMeter { get; set; } = null!;
}
