using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class Bank
{
    public short Id { get; set; }

    public string BankName { get; set; } = null!;

    public string? Icon { get; set; }

    public string CentralBankCode { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();

    public virtual ICollection<Uploader> Uploaders { get; set; } = new List<Uploader>();
}
