namespace Aban360.LocationPool.Domain.Features.MainHierarchy;

public partial class CordinalDirection
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Province> Provinces { get; set; } = new List<Province>();
}
