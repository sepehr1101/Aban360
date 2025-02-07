using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.LocationPool.Domain.Features.MainHierarchy;

[Table(nameof(ReadingBound))]
public class ReadingBound
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int ZoneId { get; set; }
    public string FromReadingNumber { get; set; }= null!;
    public string ToReadingNumber { get; set; }= null!;

    public virtual ICollection<ReadingBlock> ReadingBlocks { get; set; } = new List<ReadingBlock>();

    public virtual Zone Zone { get; set; } = null!;
}
