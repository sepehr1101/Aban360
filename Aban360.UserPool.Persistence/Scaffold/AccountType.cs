using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class AccountType
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;
}
