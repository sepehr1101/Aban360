using Aban360.ClaimPool.Domain.Features.People.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ClaimPool.Domain.Features.Draft.Entites
{
    public class RequestIndividualTag  // ToDo : Has BaseClass?
    {
        public int Id { get; set; }

        public int IndividualId { get; set; }

        public short IndividualTagDefinitionId { get; set; }

        public string? Value { get; set; }

        [ForeignKey(nameof(IndividualId))]
        public virtual RequestIndividual RequestIndividual{ get; set; } = null!;

        public virtual IndividualTagDefinition IndividualTagDefinition { get; set; } = null!;
    }
}
