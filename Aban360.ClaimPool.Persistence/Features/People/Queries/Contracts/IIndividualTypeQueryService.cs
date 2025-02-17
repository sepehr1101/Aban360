using Aban360.ClaimPool.Domain.Features.People.Entities;

namespace Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts
{
    public interface IIndividualTypeQueryService
    {
        Task<IndividualType> Get(short id);
        Task<ICollection<IndividualType>> Get();
    }
}
