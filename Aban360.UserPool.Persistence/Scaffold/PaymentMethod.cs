using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class PaymentMethod
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Icon { get; set; }

    public string? Description { get; set; }
}
