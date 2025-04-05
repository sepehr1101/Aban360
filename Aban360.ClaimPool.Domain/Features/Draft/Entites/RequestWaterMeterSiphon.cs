using Aban360.ClaimPool.Domain.Features._Base.Entities;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Draft.Entites
{
    [Table(nameof(RequestWaterMeterSiphon))]
    public class RequestWaterMeterSiphon:WaterMeterSiphonBase
    {
        [ForeignKey(nameof(SiphonId))]
        public virtual RequestSiphon  RequestSiphon{ get; set; }

        [ForeignKey(nameof(WaterMeterId))]
        public virtual RequestWaterMeter RequestWaterMeter { get; set; }    
    }
}
