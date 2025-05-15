using Aban360.ClaimPool.Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.People.Entities;

[Table(nameof(IndividualEstateRelationType), Schema = TableSchema.Name)]
public class IndividualEstateRelationType
{
    public IndividualEstateRelationTypeEnum Id { get; set; } 

    public string Title { get; set; } = null!;

    public virtual ICollection<IndividualEstate> IndividualEstates { get; set; } = new List<IndividualEstate>();
}
