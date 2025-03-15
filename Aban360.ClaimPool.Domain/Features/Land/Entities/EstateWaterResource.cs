using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Land.Entities;

[Table(nameof(EstateWaterResource))]
public class EstateWaterResource
{
    public short Id { get; set; }

    public int EstateId { get; set; }

    public short WaterResourceId { get; set; }

    public virtual Estate Estate { get; set; } = null!;

    public virtual WaterResource WaterResource { get; set; } = null!;
}
