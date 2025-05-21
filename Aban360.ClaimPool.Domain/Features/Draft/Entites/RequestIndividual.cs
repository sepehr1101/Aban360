using Aban360.ClaimPool.Domain.Features._Base.Entities;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Draft.Entites
{
    [Table(nameof(RequestIndividual))]
    public class RequestIndividual: IndividualBase
    {
        public virtual IndividualType IndividualType { get; set; } = null!;
        public virtual ICollection<RequestIndividualEstate> IndividualEstates { get; set; } = new List<RequestIndividualEstate>();

        public virtual ICollection<RequestIndividual> InversePrevious { get; set; } = new List<RequestIndividual>();

        public virtual RequestIndividual? Previous { get; set; }
        public virtual ICollection<RequestIndividualTag> IndividualTags { get; set; } = new List<RequestIndividualTag>();

        public virtual ICollection<RequestIndividualDiscountType> IndividualDiscountTypes { get; set; }= new List<RequestIndividualDiscountType>();
    }
}
