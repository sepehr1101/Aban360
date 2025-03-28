using Aban360.ClaimPool.Domain.Constants;

namespace Aban360.ClaimPool.Domain.Features._Base;

public class IndividualEstateBase
{
    public int Id { get; set; }

    public int IndividualId { get; set; }

    public int EstateId { get; set; }

    public IndividualEstateRelationEnum IndividualEstateRelationTypeId { get; set; }
}
