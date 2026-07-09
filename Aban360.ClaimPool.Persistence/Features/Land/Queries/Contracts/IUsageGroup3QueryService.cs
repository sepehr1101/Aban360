using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts
{
    public interface IUsageGroup3QueryService
    {
        Task<IEnumerable<UsageGroup3GetDto>> Get();
        Task<UsageGroup3GetDto> Get(short id);
        Task<UsageGroup3GetDto?> Get(UsageGroup3InsertDto input);
        Task<IEnumerable<UsageGroup3GetDto>> GetByParrentIds(IEnumerable<short> ids);
        Task<IEnumerable<UsageGroup3GetDto>> GetByParrentId(short id);
    }
}
