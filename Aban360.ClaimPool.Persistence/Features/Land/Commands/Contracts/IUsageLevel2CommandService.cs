using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts
{
    public interface IUsageLevel2CommandService
    {
        Task Add(UsageLevel2 usageLevel2);
        Task Remove(UsageLevel2 usageLevel2);
    }
}
