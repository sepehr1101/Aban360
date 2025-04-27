using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class RequestIndividualEstate
{
    public int Id { get; set; }

    public int IndividualId { get; set; }

    public int EstateId { get; set; }

    public short IndividualEstateRelationTypeId { get; set; }

    public virtual RequestIndividual Estate { get; set; } = null!;

    public virtual RequestIndividual Individual { get; set; } = null!;

    public virtual IndividualEstateRelationType IndividualEstateRelationType { get; set; } = null!;
}
