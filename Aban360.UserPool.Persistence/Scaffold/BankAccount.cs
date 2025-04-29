using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class BankAccount
{
    public short Id { get; set; }

    public short BankId { get; set; }

    public string Title { get; set; } = null!;

    public short AccountTypeId { get; set; }

    public int RegionId { get; set; }

    public string RegionTitle { get; set; } = null!;

    public string Iban { get; set; } = null!;

    public string Number { get; set; } = null!;

    public virtual Bank Bank { get; set; } = null!;
}
