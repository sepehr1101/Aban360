using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class WaterResource
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public short HeadquartersId { get; set; }

    public string HeadquartersTitle { get; set; } = null!;

    public virtual ICollection<EstateWaterResource> EstateWaterResources { get; set; } = new List<EstateWaterResource>();
}
