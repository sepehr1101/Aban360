using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Domain.Features.People.Entities;

namespace Aban360.ClaimPool.Domain.Features.People.Base;

public class IndividualEstateBase
{
    public int Id { get; set; }

    public int IndividualId { get; set; }

    public int EstateId { get; set; }

    public IndividualEstateRelationEnum IndividualEstateRelationTypeId { get; set; }

    public virtual EstateBase Estate { get; set; } = null!;

    public virtual IndividualBase Individual { get; set; } = null!;

    public virtual IndividualEstateRelationType IndividualEstateRelationType { get; set; } = null!;
}
