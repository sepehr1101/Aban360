using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class Document
{
    public Guid Id { get; set; }

    public Guid FileRowId { get; set; }

    public string Name { get; set; } = null!;

    public string Extension { get; set; } = null!;

    public long SizeInByte { get; set; }

    public string ContentType { get; set; } = null!;

    public byte[]? FileContent { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public string? Description { get; set; }

    public short DocumentTypeId { get; set; }

    public bool IsThumbnail { get; set; }

    public Guid? ParrentId { get; set; }

    public virtual DocumentType DocumentType { get; set; } = null!;

    public virtual ICollection<Document> InverseParrent { get; set; } = new List<Document>();

    public virtual Document? Parrent { get; set; }
}
