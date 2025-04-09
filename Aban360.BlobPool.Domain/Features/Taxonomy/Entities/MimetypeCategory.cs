using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.BlobPool.Domain.Features.Taxonomy.Entities;

[Table(nameof(MimetypeCategory))]
public partial class MimetypeCategory
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public string Name { get; set; } = null!;
}
