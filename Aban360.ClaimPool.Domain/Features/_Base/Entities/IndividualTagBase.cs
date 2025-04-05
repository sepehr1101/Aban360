using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Domain.Features._Base.Entities;

public class IndividualTagBase : IHashableEntity
{
    public int Id { get; set; }

    public int IndividualId { get; set; }

    public short IndividualTagDefinitionId { get; set; }

    public string? Value { get; set; }
    public virtual IndividualTagDefinition IndividualTagDefinition { get; set; } = null!;

}