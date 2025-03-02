using Aban360.ClaimPool.Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.People.Entities;

[Table(nameof(IndividualType), Schema = TableSchema.Name)]
public class IndividualType
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Individual> Individuals { get; set; } = new List<Individual>();
}
