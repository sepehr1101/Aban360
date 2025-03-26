using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class CompanyServiceType
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public short TariffCalculationModeId { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<CompanyService> CompanyServices { get; set; } = new List<CompanyService>();

    public virtual TariffCalculationMode TariffCalculationMode { get; set; } = null!;
}
