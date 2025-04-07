using Aban360.ClaimPool.Domain.Features.Draft.Entites;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts
{
    public interface IRequestEstateQueryService
    {
        Task<RequestEstate> Get(int id);
        Task<ICollection<RequestEstate>> Get();
    }
}
