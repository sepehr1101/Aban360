using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class Flat
{
    public int Id { get; set; }

    public int EstateId { get; set; }

    public string? PostalCode { get; set; }

    public short Storey { get; set; }

    public string? Description { get; set; }

    public virtual Estate Estate { get; set; } = null!;
}
