using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Land.Entities;

[Table(nameof(WaterResource))]
public class WaterResource
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public short HeadquartersId { get; set; }

    public string HeadquartersTitle { get; set; } = null!;

    public virtual ICollection<EstateWaterResource> EstateWaterResources { get; set; } = new List<EstateWaterResource>();
}
