using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class RequestIndividualDiscountType
{
    public int Id { get; set; }

    public int IndividualId { get; set; }

    public short DiscountTypeId { get; set; }

    public Guid? UserId { get; set; }

    public DateTime ExpireDate { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    public string InsertLogInfo { get; set; } = null!;

    public string? RemoveLogInfo { get; set; }

    public string Hash { get; set; } = null!;

    public virtual DiscountType DiscountType { get; set; } = null!;

    public virtual RequestIndividual Individual { get; set; } = null!;
}
