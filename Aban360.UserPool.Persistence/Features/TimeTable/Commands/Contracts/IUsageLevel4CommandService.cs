using Aban360.UserPool.Domain.Features.TimeTable.Entites;

namespace Aban360.UserPool.Persistence.Features.TimeTable.Commands.Contracts
{
    public interface IUsageLevel4CommandService
    {
        Task Add(UsageLevel4 usageLevel4);
        Task Remove(UsageLevel4 usageLevel4);
    }
}
