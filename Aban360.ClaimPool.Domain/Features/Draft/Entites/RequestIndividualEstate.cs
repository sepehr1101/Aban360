using Aban360.ClaimPool.Domain.Features._Base.Entities;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Draft.Entites
{
    public class RequestIndividualEstate:IndividualEstateBase
    {
        [ForeignKey(nameof(EstateId))]
        public virtual RequestEstate RequestEstate{ get; set; } = null!;

        [ForeignKey(nameof(IndividualId))]
        public virtual RequestIndividual RequestIndividual{ get; set; } = null!;

        public virtual IndividualEstateRelationType IndividualEstateRelationType { get; set; } = null!;

    }
}
