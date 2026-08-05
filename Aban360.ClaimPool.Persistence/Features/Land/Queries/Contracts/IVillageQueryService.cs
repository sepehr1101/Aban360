using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts
{
    public interface IVillageQueryService
    {
        Task<IEnumerable<VillageGetDto>> Get();
    }
}
