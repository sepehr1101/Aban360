using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class DynamicReport
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public long Version { get; set; }

    public string? Description { get; set; }

    public Guid UserId { get; set; }

    public string UserName { get; set; } = null!;

    public Guid DocumentId { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    public string InsertLogInfo { get; set; } = null!;

    public string? RemoveLogInfo { get; set; }

    public string Hash { get; set; } = null!;
}
