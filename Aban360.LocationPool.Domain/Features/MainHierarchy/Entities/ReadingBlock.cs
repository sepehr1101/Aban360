using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;

[Table(nameof(ReadingBlock))]
public class ReadingBlock
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public int ReadingBoundId { get; set; }

    public string FromReadingNumber { get; set; } = null!;

    public string ToReadingNumber { get; set; } = null!;

    public virtual ReadingBound ReadingBound { get; set; } = null!;
}
