using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;

namespace Aban360.ClaimPool.Domain.Features.Draft.Entites
{
    public class RequestUser
    {
        public RequestEstate RequestEstate { get; set; }
        public RequestFlat RequestFlat { get; set; }
        public RequestIndividual RequestIndividual { get; set; }
        public RequestSiphon RequestSiphon { get; set; }
        public RequestWaterMeter RequestWaterMeter { get; set; }
        public RequestWaterMeterSiphon RequestWaterMeterSiphon { get; set; }
        public RequestWaterMeterTag RequestWaterMeterTag { get; set; }
        public RequestIndividualEstate RequestIndividualEstate { get; set; }    
        public RequestIndividualTag RequestIndividualTag { get; set; }

    }
}
