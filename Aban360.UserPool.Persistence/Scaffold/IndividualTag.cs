using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class IndividualTag
{
    public int Id { get; set; }

    public int IndividualId { get; set; }

    public short IndividualTagDefinitionId { get; set; }

    public string? Value { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    public string InsertLogInfo { get; set; } = null!;

    public string? RemoveLogInfo { get; set; }

    public string Hash { get; set; } = null!;

    public virtual Individual Individual { get; set; } = null!;

    public virtual IndividualTagDefinition IndividualTagDefinition { get; set; } = null!;
}
