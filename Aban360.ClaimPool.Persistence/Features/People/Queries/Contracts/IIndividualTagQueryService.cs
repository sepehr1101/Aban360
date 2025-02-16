using Aban360.ClaimPool.Domain.Features.People.Entities;

namespace Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts
{
    public interface IIndividualTagQueryService
    {
        Task<ICollection<IndividualTag>> Get(string nationalId);
        Task<IndividualTag> Get(int id);
    }
}
