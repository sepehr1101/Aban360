using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class WaterMeterTagDefinition
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Color { get; set; }

    public virtual ICollection<RequestWaterMeterTag> RequestWaterMeterTags { get; set; } = new List<RequestWaterMeterTag>();

    public virtual ICollection<WaterMeterTag> WaterMeterTags { get; set; } = new List<WaterMeterTag>();
}
