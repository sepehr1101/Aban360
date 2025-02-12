using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts
{
    public interface IUsageCommandSevice
    {
        Task Add(Usage usage);
        Task Remove(Usage usage);
    }
}
