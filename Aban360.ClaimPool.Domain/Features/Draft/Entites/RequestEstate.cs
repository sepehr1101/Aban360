using Aban360.ClaimPool.Domain.Features._Base.Entities;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Draft.Entites
{
    [Table(nameof(RequestEstate))]
    public class RequestEstate: EstateBase
    {
        public virtual ConstructionType ConstructionType { get; set; } = null!;

        public virtual EstateBoundType EstateBoundType { get; set; } = null!;

        public virtual ICollection<RequestFlat> Flats{ get; set; } = new List<RequestFlat>();

        public virtual ICollection<RequestIndividualEstate> IndividualEstates{ get; set; } = new List<RequestIndividualEstate>();

        //public virtual ICollection<RequestEstate> InversePrevious { get; set; } = new List<RequestEstate>();

        //public virtual Estate? Previous { get; set; }

        public virtual Usage UsageConsumtion { get; set; } = null!;

        public virtual Usage UsageSell { get; set; } = null!;
        public virtual ICollection<RequestWaterMeter> WaterMeters { get; set; } = new List<RequestWaterMeter>();
    }
}
