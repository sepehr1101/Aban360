using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class DeepLog
{
    public long Id { get; set; }

    public int OperationTypeId { get; set; }

    public string PrimaryDb { get; set; } = null!;

    public string PrimaryTable { get; set; } = null!;

    public string PrimaryId { get; set; } = null!;

    public string? ValueBefore { get; set; }

    public string? ValueAfter { get; set; }

    public string Ip { get; set; } = null!;

    public DateTime InsertDateTime { get; set; }

    public Guid UserId { get; set; }

    public string ClientInfo { get; set; } = null!;

    public virtual OperationType OperationType { get; set; } = null!;
}
