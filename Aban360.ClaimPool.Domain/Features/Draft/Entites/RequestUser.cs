using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;

namespace Aban360.ClaimPool.Domain.Features.Draft.Entites
{
    public class RequestUser
    {
        public RequestEstate RequestEstate { get; set; }//


        //public RequestFlat RequestFlat { get; set; }
        public ICollection<RequestFlat> RequestFlat { get; set; }

        //public RequestIndividual RequestIndividual { get; set; }
        public ICollection<RequestIndividual> RequestIndividual { get; set; }


        //public RequestSiphon RequestSiphon { get; set; }
        public ICollection<RequestSiphon> RequestSiphon { get; set; }


        public RequestWaterMeter RequestWaterMeter { get; set; }//



        //public RequestWaterMeterSiphon RequestWaterMeterSiphon { get; set; }
        public ICollection<RequestWaterMeterSiphon> RequestWaterMeterSiphon { get; set; }


        //public RequestWaterMeterTag RequestWaterMeterTag { get; set; }
        public ICollection<RequestWaterMeterTag> RequestWaterMeterTag { get; set; }


        //public RequestIndividualEstate RequestIndividualEstate { get; set; }    
        public ICollection<RequestIndividualEstate> RequestIndividualEstate { get; set; }


        //public RequestIndividualTag RequestIndividualTag { get; set; }
        public ICollection<RequestIndividualTag> RequestIndividualTag { get; set; }

    }
}
