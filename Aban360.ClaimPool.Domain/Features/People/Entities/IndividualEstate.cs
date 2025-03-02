using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.People.Entities;

[Table(nameof(IndividualEstate), Schema = TableSchema.Name)]
public class IndividualEstate
{
    public int Id { get; set; }

    public int IndividualId { get; set; }

    public int EstateId { get; set; }

    public IndividualEstateRelationEnum IndividualEstateRelationTypeId { get; set; }

    public virtual Estate Estate { get; set; } = null!;

    public virtual Individual Individual { get; set; } = null!;

    public virtual IndividualEstateRelationType IndividualEstateRelationType { get; set; } = null!;
}
