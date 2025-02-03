using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.LocationPool.Domain.Features.MainHierarchy;

[Table(nameof(ReadingBlock))]
public class ReadingBlock
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public short ReadingBoundId { get; set; }

    public virtual ReadingBound ReadingBound { get; set; } = null!;
}
