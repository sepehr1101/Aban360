using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class Tariff
{
    public int Id { get; set; }

    public short LineItemTypeId { get; set; }

    public short OfferingId { get; set; }

    public string Condition { get; set; } = null!;

    public string Formula { get; set; } = null!;

    public string FromDateJalali { get; set; } = null!;

    public string ToDateJalali { get; set; } = null!;

    public string? Description { get; set; }

    public virtual LineItemType LineItemType { get; set; } = null!;

    public virtual Offering Offering { get; set; } = null!;
}
