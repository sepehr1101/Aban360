using Aban360.ClaimPool.Domain.Features._Base;

namespace Aban360.ClaimPool.Domain.Features.Draft.Entites
{
    public class RequestWaterMeterSiphon:WaterMeterSiphonBase
    {
        public virtual RequestSiphon  RequestSiphon{ get; set; }
        public virtual RequestWaterMeter RequestWaterMeter { get; set; }    
    }
}
