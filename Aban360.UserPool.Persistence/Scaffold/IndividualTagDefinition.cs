using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class IndividualTagDefinition
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Color { get; set; }

    public virtual ICollection<IndividualTag> IndividualTags { get; set; } = new List<IndividualTag>();
}
