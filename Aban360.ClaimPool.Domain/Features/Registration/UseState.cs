using System;
using System.Collections.Generic;

namespace Aban360.ClaimPool.Domain.Features.Registration;

public partial class UseState
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
