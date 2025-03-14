using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class Province
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public short CordinalDirectionId { get; set; }

    public short CountryId { get; set; }

    public virtual CordinalDirection CordinalDirection { get; set; } = null!;

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<Headquarter> Headquarters { get; set; } = new List<Headquarter>();
}
