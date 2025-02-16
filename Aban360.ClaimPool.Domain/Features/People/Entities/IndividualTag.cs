using Aban360.Common.BaseEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.People.Entities;

[Table(nameof(IndividualTag))]
public class IndividualTag: IHashableEntity
{
    public int Id { get; set; }

    public int IndividualId { get; set; }

    public short IndividualTagDefinitionId { get; set; }

    public string? Value { get; set; }

    public virtual Individual Individual { get; set; } = null!;

    public virtual IndividualTagDefinition IndividualTagDefinition { get; set; } = null!;
}
