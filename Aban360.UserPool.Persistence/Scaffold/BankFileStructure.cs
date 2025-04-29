using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class BankFileStructure
{
    public short Id { get; set; }

    public short FromIndex { get; set; }

    public short ToIndex { get; set; }

    public short StringLenght { get; set; }

    public string Title { get; set; } = null!;

    public bool IsHeader { get; set; }
}
