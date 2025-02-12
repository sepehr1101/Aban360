using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts
{
    public interface IUsageGetAllHandler
    {
        Task<ICollection<UsageGetDto>> Handle();
    }
}
