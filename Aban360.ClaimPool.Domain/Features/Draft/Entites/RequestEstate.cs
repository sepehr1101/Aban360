using Aban360.ClaimPool.Domain.Features._Base;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Domain.Features.People.Entities;

namespace Aban360.ClaimPool.Domain.Features.Draft.Entites
{
    public class RequestEstate: EstateBase
    {
        public virtual ConstructionType ConstructionType { get; set; } = null!;

        public virtual EstateBoundType EstateBoundType { get; set; } = null!;

        //public virtual ICollection<Flat> Flats { get; set; } = new List<Flat>(); //TODO: RequestFlat Entity

        //public virtual ICollection<IndividualEstate> IndividualEstates { get; set; } = new List<IndividualEstate>();

        public virtual ICollection<RequestEstate> InversePrevious { get; set; } = new List<RequestEstate>();

        public virtual Estate? Previous { get; set; }

        public virtual Usage UsageConsumtion { get; set; } = null!;

        public virtual Usage UsageSell { get; set; } = null!;
        public virtual ICollection<RequestWaterMeter> WaterMeters { get; set; } = new List<RequestWaterMeter>();
    }
}
