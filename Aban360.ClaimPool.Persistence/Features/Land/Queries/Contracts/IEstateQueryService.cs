using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts
{
    public interface IEstateQueryService
    {
        Task<Estate> Get(int id);
        Task<ICollection<Estate>> Get();
    }
}
