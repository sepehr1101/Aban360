using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts
{
    public interface IEstateWaterResourceCommandService
    {
        Task Add(EstateWaterResource estateWaterResource);
        Task Remove(EstateWaterResource estateWaterResource);
    }
}
