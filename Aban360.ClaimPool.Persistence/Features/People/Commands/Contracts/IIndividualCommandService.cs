using Aban360.ClaimPool.Domain.Features.People.Base;
using Aban360.ClaimPool.Domain.Features.People.Entities;

namespace Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts
{
    public interface IIndividualCommandService
    {
        Task Add(Individual individual);
        Task Add(ICollection<Individual> individuals);
        Task Remove(Individual individual);
    }
}
