using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class WaterMeterChangeNumberHistory
{
    public long Id { get; set; }

    public int Consumption { get; set; }

    public float ConstumptionAverage { get; set; }

    public short ChangeMeterReasonId { get; set; }

    public long InvoiceId { get; set; }

    public virtual Invoice Invoice { get; set; } = null!;
}
