using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class Usage
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public short ProvinceId { get; set; }

    public virtual ICollection<Estate> EstateUsageConsumtions { get; set; } = new List<Estate>();

    public virtual ICollection<Estate> EstateUsageSells { get; set; } = new List<Estate>();

    public virtual ICollection<Guild> Guilds { get; set; } = new List<Guild>();

    public virtual ICollection<RequestEstate> RequestEstateUsageConsumtions { get; set; } = new List<RequestEstate>();

    public virtual ICollection<RequestEstate> RequestEstateUsageSells { get; set; } = new List<RequestEstate>();
}
