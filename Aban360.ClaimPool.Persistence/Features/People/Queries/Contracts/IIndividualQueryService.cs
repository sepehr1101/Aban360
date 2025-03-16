using Aban360.ClaimPool.Domain.Features.People.Base;
using Aban360.ClaimPool.Domain.Features.People.Entities;

namespace Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts
{
    public interface IIndividualQueryService
    {
        Task<Individual> Get(int id);
        Task<ICollection<Individual>> Get();
    }
}
