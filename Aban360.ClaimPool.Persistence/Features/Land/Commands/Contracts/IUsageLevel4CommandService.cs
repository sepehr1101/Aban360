using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts
{
    public interface IUsageLevel4CommandService
    {
        Task Add(UsageLevel4 usageLevel4);
        Task Remove(UsageLevel4 usageLevel4);
    }
}
