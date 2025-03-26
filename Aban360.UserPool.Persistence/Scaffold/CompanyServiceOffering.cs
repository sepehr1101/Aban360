using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class CompanyServiceOffering
{
    public short CompanyServiceId { get; set; }

    public short OfferingId { get; set; }

    public short Id { get; set; }

    public virtual CompanyService CompanyService { get; set; } = null!;

    public virtual Offering Offering { get; set; } = null!;
}
