using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts
{
    public interface IUsageLevel1CommandService
    {
        Task Add(UsageLevel1 usageLevel1);
        Task Remove(UsageLevel1 usageLevel1);
    }
}
