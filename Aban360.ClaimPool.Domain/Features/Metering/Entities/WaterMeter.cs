using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Metering.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Metering.Entities
{
    [Table(nameof(WaterMeter), Schema = TableSchema.Name)]
    public class WaterMeter: WaterMeterBase
    {
        
    }
}
