using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts
{
    public interface IUsageLevel3CommandService
    {
        Task Add(UsageLevel3 usageLevel3);
        Task Remove(UsageLevel3 usageLevel3);
    }
}
