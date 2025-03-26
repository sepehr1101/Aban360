using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class CounterState
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public short ClientOrder { get; set; }

    public bool EnterNumberOption { get; set; }

    public bool NumberRequired { get; set; }

    public bool NonReadable { get; set; }

    public bool NumberLessThanPre { get; set; }

    public bool IsChanged { get; set; }

    public bool IsBroken { get; set; }

    public bool IsNull { get; set; }

    public bool IsEnabled { get; set; }

    public bool ImageRequired { get; set; }

    public short HeadquartersId { get; set; }

    public string HeadquartersTitle { get; set; } = null!;
}
