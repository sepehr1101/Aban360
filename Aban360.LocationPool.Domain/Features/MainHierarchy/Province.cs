using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.LocationPool.Domain.Features.MainHierarchy;

[Table(nameof(Province))]
public class Province
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public short CordinalDirectionId { get; set; }
    public short CountryId { get; set; }

    public virtual CordinalDirection CordinalDirection { get; set; } = null!;
    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<Headquarters> Headquarters { get; set; } = new List<Headquarters>();
}
