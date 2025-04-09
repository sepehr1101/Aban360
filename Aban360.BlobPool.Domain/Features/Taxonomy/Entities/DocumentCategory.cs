using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.BlobPool.Domain.Features.Taxonomy.Entities;

[Table(nameof(DocumentCategory))]
public class DocumentCategory
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Icon { get; set; }

    public string? Css { get; set; }

    public virtual ICollection<DocumentType> DocumentTypes { get; set; } = new List<DocumentType>();
}
