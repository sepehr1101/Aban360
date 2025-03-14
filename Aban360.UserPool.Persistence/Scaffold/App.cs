using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class App
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Style { get; set; } = null!;

    public bool InMenu { get; set; }

    public int LogicalOrder { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Module> Modules { get; set; } = new List<Module>();
}
