using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class WaterMeterTag
{
    public int Id { get; set; }

    public int WaterMeterId { get; set; }

    public short WaterMeterTagDefinitionId { get; set; }

    public string? Value { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    public string InsertLogInfo { get; set; } = null!;

    public string? RemoveLogInfo { get; set; }

    public string Hash { get; set; } = null!;

    public virtual WaterMeter WaterMeter { get; set; } = null!;

    public virtual WaterMeterTagDefinition WaterMeterTagDefinition { get; set; } = null!;
}
