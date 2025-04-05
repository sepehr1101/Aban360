using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features._Base.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Metering.Entities;

[Table(nameof(WaterMeterTag), Schema = TableSchema.Name)]
public class WaterMeterTag:WaterMeterTagBase
{
    [ForeignKey(nameof(WaterMeterId))]
    public virtual WaterMeter WaterMeter { get; set; } = null!;

}
