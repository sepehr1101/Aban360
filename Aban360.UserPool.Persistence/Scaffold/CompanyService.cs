using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class CompanyService
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public bool IsMultiSelect { get; set; }

    public short CompanyServiceTypeId { get; set; }

    public virtual ICollection<CompanyServiceOffering> CompanyServiceOfferings { get; set; } = new List<CompanyServiceOffering>();

    public virtual CompanyServiceType CompanyServiceType { get; set; } = null!;
}
