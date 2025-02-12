using Aban360.ClaimPool.Domain.Features.Land;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.People;

[Table(nameof(IndividualEstateRelationType))]
public partial class IndividualEstateRelationType
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<IndividualEstate> IndividualEstates { get; set; } = new List<IndividualEstate>();
}
