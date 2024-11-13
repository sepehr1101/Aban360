using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class OperationType
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Css { get; set; } = null!;

    public virtual ICollection<DeepLog> DeepLogs { get; set; } = new List<DeepLog>();
}
