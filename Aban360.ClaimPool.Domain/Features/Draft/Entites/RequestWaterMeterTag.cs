using Aban360.ClaimPool.Domain.Features._Base.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Draft.Entites;

[Table(nameof(RequestWaterMeterTag))]
public partial class RequestWaterMeterTag:WaterMeterTagBase
{
    [ForeignKey(nameof(WaterMeterId))]
    public virtual RequestWaterMeter RequestWaterMeter { get; set; } = null!;

}
