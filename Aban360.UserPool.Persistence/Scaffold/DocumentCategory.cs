using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class DocumentCategory
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Icon { get; set; }

    public string? Css { get; set; }

    public virtual ICollection<DocumentType> DocumentTypes { get; set; } = new List<DocumentType>();
}
