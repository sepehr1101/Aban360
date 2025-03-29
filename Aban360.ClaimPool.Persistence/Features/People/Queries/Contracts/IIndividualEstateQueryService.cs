using Aban360.ClaimPool.Domain.Features.People.Entities;

namespace Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts
{
    public interface IIndividualEstateQueryService
    {
        Task<IndividualEstate> Get(int id);
        Task<ICollection<IndividualEstate>> Get();
    }
}
