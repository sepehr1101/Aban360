using Aban360.ClaimPool.Domain.Features._Base;
using Aban360.ClaimPool.Domain.Features.People.Entities;

namespace Aban360.ClaimPool.Domain.Features.Draft.Entites
{
    public class RequestIndividual: IndividualBase
    {
        public virtual RequestWaterMeter RequestWaterMeter { get; set; }
        public virtual IndividualType IndividualType { get; set; } = null!;
        public virtual ICollection<RequestIndividualEstate> RequestIndividualEstates { get; set; } = new List<RequestIndividualEstate>();

        public virtual ICollection<RequestIndividual> InversePrevious { get; set; } = new List<RequestIndividual>();

        public virtual RequestIndividual? Previous { get; set; }
        public virtual ICollection<RequestIndividualTag> RequestIndividualTags { get; set; } = new List<RequestIndividualTag>();
    }
}
