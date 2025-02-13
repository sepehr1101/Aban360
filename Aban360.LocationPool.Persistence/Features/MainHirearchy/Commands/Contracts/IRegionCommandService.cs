using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;

namespace Aban360.LocationPool.Persistence.Features.MainHirearchy.Commands.Contracts
{
    public interface IRegionCommandService
    {
        Task Add(Region region);
        Task Remove(Region region);
    }
}
