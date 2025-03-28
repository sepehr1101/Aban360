using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features._Base;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.People.Entities;

[Table(nameof(IndividualEstate), Schema = TableSchema.Name)]
public class IndividualEstate: IndividualEstateBase
{
    public virtual Estate Estate { get; set; } = null!;

    public virtual Individual Individual { get; set; } = null!;

    public virtual IndividualEstateRelationType IndividualEstateRelationType { get; set; } = null!;
}
