using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class EstateWaterResource
{
    public short Id { get; set; }

    public int EstateId { get; set; }

    public short WaterResourceId { get; set; }

    public virtual Estate Estate { get; set; } = null!;

    public virtual WaterResource WaterResource { get; set; } = null!;
}
