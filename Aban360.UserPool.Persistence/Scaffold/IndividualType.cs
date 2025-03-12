using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class IndividualType
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Individual> Individuals { get; set; } = new List<Individual>();
}
