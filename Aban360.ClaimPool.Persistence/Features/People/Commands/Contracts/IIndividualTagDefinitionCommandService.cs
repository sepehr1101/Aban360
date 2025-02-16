using Aban360.ClaimPool.Domain.Features.People.Entities;

namespace Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts
{
    public interface IIndividualTagDefinitionCommandService
    {
        Task Add(IndividualTagDefinition individualTagDefinition);
        Task Remove(IndividualTagDefinition individualTagDefinition);
    }
}
