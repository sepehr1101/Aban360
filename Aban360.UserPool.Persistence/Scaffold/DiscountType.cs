using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class DiscountType
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<IndividualDiscountType> IndividualDiscountTypes { get; set; } = new List<IndividualDiscountType>();

    public virtual ICollection<RequestIndividualDiscountType> RequestIndividualDiscountTypes { get; set; } = new List<RequestIndividualDiscountType>();
}
