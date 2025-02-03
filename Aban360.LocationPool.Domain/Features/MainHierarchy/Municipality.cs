using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.LocationPool.Domain.Features.MainHierarchy;

[Table(nameof(Municipality))]
public class Municipality
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public short ZoneId { get; set; }

    public virtual Zone Zone { get; set; } = null!;
}
