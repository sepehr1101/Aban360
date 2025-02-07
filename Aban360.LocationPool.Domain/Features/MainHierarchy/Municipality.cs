using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.LocationPool.Domain.Features.MainHierarchy;

[Table(nameof(Municipality))]
public class Municipality
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int ZoneId { get; set; }
    public bool IsVillage { get; set; }

    public virtual Zone Zone { get; set; } = null!;
}
