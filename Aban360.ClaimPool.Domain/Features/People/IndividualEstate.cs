using Aban360.ClaimPool.Domain.Features.Land;
using System;
using System.Collections.Generic;

namespace Aban360.ClaimPool.Domain.Features.People;

public partial class IndividualEstate
{
    public int Id { get; set; }

    public int IndividualId { get; set; }

    public int EstateId { get; set; }

    public short IndividualEstateRelationTypeId { get; set; }

    public virtual Estate Estate { get; set; } = null!;

    public virtual Individual Individual { get; set; } = null!;

    public virtual IndividualEstateRelationType IndividualEstateRelationType { get; set; } = null!;
}
