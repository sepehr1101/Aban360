using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Domain.Features._Base.Entities;

public class WaterMeterTagBase : IHashableEntity
{
    public int Id { get; set; }

    public int WaterMeterId { get; set; }

    public short WaterMeterTagDefinitionId { get; set; }

    public string? Value { get; set; }
    public virtual WaterMeterTagDefinition WaterMeterTagDefinition { get; set; } = null!;

}