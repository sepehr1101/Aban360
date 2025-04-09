using Aban360.ClaimPool.Domain.Features.Draft.Entites;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts
{
    public interface IRequestIndividualEstateQueryService
    {
        Task<RequestIndividualEstate> Get(int id);
        Task<ICollection<RequestIndividualEstate>> Get();
    }
}
