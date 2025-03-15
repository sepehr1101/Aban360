using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts
{
    public interface IWaterResourceCommandService
    {
        Task Add(WaterResource waterResource);
        Task Remove(WaterResource waterResource);
    }
}
