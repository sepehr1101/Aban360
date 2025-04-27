using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class RequestFlat
{
    public int Id { get; set; }

    public int EstateId { get; set; }

    public string? PostalCode { get; set; }

    public short Storey { get; set; }

    public string? Description { get; set; }

    public virtual RequestEstate Estate { get; set; } = null!;
}
