using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts
{
    public interface IConstructionTypeQueryService
    {
        Task<ConstructionType> Get(short id);
        Task<ICollection<ConstructionType>> Get();
    }
}

