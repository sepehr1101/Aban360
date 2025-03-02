using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts
{
    public interface IEstateBoundTypeQueryService
    {
        Task<EstateBoundType> Get(short id);
        Task<ICollection<EstateBoundType>> Get();
    }
}
