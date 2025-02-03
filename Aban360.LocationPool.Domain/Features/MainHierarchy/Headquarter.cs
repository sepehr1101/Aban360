using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.LocationPool.Domain.Features.MainHierarchy;

[Table(nameof(Headquarter))]
public class Headquarter
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public short ProvinceId { get; set; }

    public virtual Province Province { get; set; } = null!;

    public virtual ICollection<Region> Regions { get; set; } = new List<Region>();
}
