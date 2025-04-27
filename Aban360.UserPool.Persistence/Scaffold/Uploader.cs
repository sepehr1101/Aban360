using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class Uploader
{
    public short Id { get; set; }

    public Guid UserId { get; set; }

    public string Username { get; set; } = null!;

    public short BankId { get; set; }

    public DateTime InsertDateTime { get; set; }

    public int InsertRecordCount { get; set; }

    public long Amount { get; set; }

    public virtual Bank Bank { get; set; } = null!;

    public virtual ICollection<Credit> Credits { get; set; } = new List<Credit>();
}
