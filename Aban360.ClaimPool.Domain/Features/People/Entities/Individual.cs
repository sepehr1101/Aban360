using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.People.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.People.Entities;

[Table(nameof(Individual), Schema = TableSchema.Name)]
public class Individual: IndividualBase
{
    public virtual IndividualType IndividualType { get; set; } = null!;
    public virtual ICollection<IndividualEstate> IndividualEstates { get; set; } = new List<IndividualEstate>();

    public virtual ICollection<Individual> InversePrevious { get; set; } = new List<Individual>();

    public virtual Individual? Previous { get; set; }
    public virtual ICollection<IndividualTag> IndividualTags { get; set; } = new List<IndividualTag>();
}
