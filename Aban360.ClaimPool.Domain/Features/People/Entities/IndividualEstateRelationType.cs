using Aban360.ClaimPool.Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.People.Entities;

[Table(nameof(IndividualEstateRelationType))]
public class IndividualEstateRelationType
{
    public IndividualEstateRelationEnum Id { get; set; } 

    public string Title { get; set; } = null!;

    public virtual ICollection<IndividualEstate> IndividualEstates { get; set; } = new List<IndividualEstate>();
}
