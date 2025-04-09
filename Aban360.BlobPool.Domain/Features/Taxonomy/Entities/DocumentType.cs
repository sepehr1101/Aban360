using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.BlobPool.Domain.Features.Taxonomy.Entities;

[Table(nameof(DocumentType))]
public class DocumentType
{
    public short Id { get; set; }

    public short DocumentCategoryId { get; set; }

    public string Title { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Icon { get; set; } = null!;

    public string Css { get; set; } = null!;

    public virtual DocumentCategory DocumentCategory { get; set; } = null!;
}
