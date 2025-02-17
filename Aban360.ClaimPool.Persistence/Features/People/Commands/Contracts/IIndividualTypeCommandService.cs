using Aban360.ClaimPool.Domain.Features.People.Entities;

namespace Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts
{
    public interface IIndividualTypeCommandService
    {
        Task Add(IndividualType individual);
        Task Remove(IndividualType individual);
    }
}
