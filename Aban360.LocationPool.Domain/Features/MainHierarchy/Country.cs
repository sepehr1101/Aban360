using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.LocationPool.Domain.Features.MainHierarchy;

[Table(nameof(Country))]
public class Country
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;
}
