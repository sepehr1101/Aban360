using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class TariffCalculationMode
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<CompanyServiceType> CompanyServiceTypes { get; set; } = new List<CompanyServiceType>();
}
