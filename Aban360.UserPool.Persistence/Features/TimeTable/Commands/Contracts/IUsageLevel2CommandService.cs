using Aban360.UserPool.Domain.Features.TimeTable.Entites;

namespace Aban360.UserPool.Persistence.Features.TimeTable.Commands.Contracts
{
    public interface IUsageLevel2CommandService
    {
        Task Add(UsageLevel2 usageLevel2);
        Task Remove(UsageLevel2 usageLevel2);
    }
}
