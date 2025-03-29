using Aban360.ClaimPool.Domain.Features._Base;
using Aban360.ClaimPool.Domain.Features.People.Entities;

namespace Aban360.ClaimPool.Domain.Features.Draft.Entites
{
    public class RequestIndividualEstate:IndividualEstateBase
    {
        public virtual RequestEstate RequestEstate{ get; set; } = null!;

        public virtual RequestIndividual RequestIndividual{ get; set; } = null!;

        public virtual IndividualEstateRelationType IndividualEstateRelationType { get; set; } = null!;

    }
}
