using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.LocationPool.Domain.Features.MainHierarchy;

[Table(nameof(Zone))]
public class Zone
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int RegionId { get; set; }

    public string UnstandardCode { get; set; } = null!;

    public virtual ICollection<Municipality> Municipalities { get; set; } = new List<Municipality>();

    public virtual ICollection<ReadingBound> ReadingBounds { get; set; } = new List<ReadingBound>();

    public virtual Region Region { get; set; } = null!;
}
