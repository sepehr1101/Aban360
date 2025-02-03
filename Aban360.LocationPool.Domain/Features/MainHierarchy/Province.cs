using System;
using System.Collections.Generic;

namespace Aban360.LocationPool.Domain.Features.MainHierarchy;

public partial class Province
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public short CordinalDirectionId { get; set; }

    public virtual CordinalDirection CordinalDirection { get; set; } = null!;

    public virtual ICollection<Headquarter> Headquarters { get; set; } = new List<Headquarter>();
}
