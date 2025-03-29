using Aban360.ClaimPool.Domain.Features.People.Entities;

namespace Aban360.ClaimPool.Domain.Features.Draft.Entites
{
    public class RequestIndividualTag  // ToDo : Has Base?
    {
        public int Id { get; set; }

        public int RequestIndividualId { get; set; }

        public short IndividualTagDefinitionId { get; set; }

        public string? Value { get; set; }

        public virtual RequestIndividual RequestIndividual{ get; set; } = null!;

        public virtual IndividualTagDefinition IndividualTagDefinition { get; set; } = null!;
    }
}
