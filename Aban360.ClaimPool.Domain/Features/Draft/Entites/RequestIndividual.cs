using Aban360.ClaimPool.Domain.Features._Base;
using Aban360.ClaimPool.Domain.Features.People.Entities;

namespace Aban360.ClaimPool.Domain.Features.Draft.Entites
{
    public class RequestIndividual: IndividualBase
    {
        public virtual IndividualType IndividualType { get; set; } = null!;
        //public virtual ICollection<IndividualEstate> IndividualEstates { get; set; } = new List<IndividualEstate>();

        public virtual ICollection<RequestIndividual> InversePrevious { get; set; } = new List<RequestIndividual>();

        public virtual RequestIndividual? Previous { get; set; }
        //public virtual ICollection<IndividualTag> IndividualTags { get; set; } = new List<IndividualTag>();
    }
}
