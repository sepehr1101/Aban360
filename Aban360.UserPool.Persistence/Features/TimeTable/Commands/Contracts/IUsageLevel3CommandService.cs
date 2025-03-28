using Aban360.UserPool.Domain.Features.TimeTable.Entites;

namespace Aban360.UserPool.Persistence.Features.TimeTable.Commands.Contracts
{
    public interface IUsageLevel3CommandService
    {
        Task Add(UsageLevel3 usageLevel3);
        Task Remove(UsageLevel3 usageLevel3);
    }
}
