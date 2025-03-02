using Aban360.ClaimPool.Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.People.Entities;

[Table(nameof(IndividualTagDefinition), Schema = TableSchema.Name)]
public class IndividualTagDefinition
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Color { get; set; }

    public virtual ICollection<IndividualTag> IndividualTags { get; set; } = new List<IndividualTag>();
}
