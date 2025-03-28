using Aban360.UserPool.Domain.Features.TimeTable.Entites;

namespace Aban360.UserPool.Persistence.Features.TimeTable.Commands.Contracts
{
    public interface IUsageLevel1CommandService
    {
        Task Add(UsageLevel1 usageLevel1);
        Task Remove(UsageLevel1 usageLevel1);
    }
}
