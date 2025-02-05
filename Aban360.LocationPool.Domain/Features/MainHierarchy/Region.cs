﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.LocationPool.Domain.Features.MainHierarchy;

[Table(nameof(Region))]
public partial class Region
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public short HeadquartersId { get; set; }

    public virtual Headquarter Headquarters { get; set; } = null!;

    public virtual ICollection<Zone> Zones { get; set; } = new List<Zone>();
}
