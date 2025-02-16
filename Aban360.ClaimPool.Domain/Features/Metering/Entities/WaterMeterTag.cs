using Aban360.Common.BaseEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Metering.Entities;

[Table(nameof(WaterMeterTag))]
public class WaterMeterTag:IHashableEntity
{
    public int Id { get; set; }

    public int WaterMeterId { get; set; }

    public short WaterMeterTagDefinitionId { get; set; }

    public string? Value { get; set; }

    public virtual WaterMeter WaterMeter { get; set; } = null!;

    public virtual WaterMeterTagDefinition WaterMeterTagDefinition { get; set; } = null!;
}
