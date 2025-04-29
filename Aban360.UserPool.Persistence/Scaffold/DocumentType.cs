using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class DocumentType
{
    public short Id { get; set; }

    public short DocumentCategoryId { get; set; }

    public string Title { get; set; } = null!;

    public string Icon { get; set; } = null!;

    public string Css { get; set; } = null!;

    public virtual DocumentCategory DocumentCategory { get; set; } = null!;

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
}
