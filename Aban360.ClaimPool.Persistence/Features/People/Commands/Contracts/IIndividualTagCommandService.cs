using Aban360.ClaimPool.Domain.Features.People.Entities;

namespace Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts
{
    public interface IIndividualTagCommandService
    {
        Task Add(IndividualTag individualTag);
        Task Remove(IndividualTag individualTag);
    }
}
