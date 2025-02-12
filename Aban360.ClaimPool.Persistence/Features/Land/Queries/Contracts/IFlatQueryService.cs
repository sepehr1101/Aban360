using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts
{
    public interface IFlatQueryService
    {
        Task<Flat> Get(int id);
        Task<ICollection<Flat>> Get();
    }
}
