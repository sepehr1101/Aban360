using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.LocationPool.Domain.Features.MainHierarchy;

[Table(nameof(ReadingBound))]
public class ReadingBound
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public int ZoneId { get; set; }

    public virtual ICollection<ReadingBlock> ReadingBlocks { get; set; } = new List<ReadingBlock>();

    public virtual Zone Zone { get; set; } = null!;
}
