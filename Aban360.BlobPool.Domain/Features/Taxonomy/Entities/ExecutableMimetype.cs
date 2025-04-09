using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.BlobPool.Domain.Features.Taxonomy.Entities;

[Table(nameof(ExecutableMimetype))]
public class ExecutableMimetype
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public bool StreamingOption { get; set; }

    public bool FrontendExecutor { get; set; }
}
