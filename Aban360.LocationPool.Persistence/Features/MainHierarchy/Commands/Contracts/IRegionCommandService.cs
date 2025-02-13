using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts
{
    public interface IRegionCommandService
    {
        Task Add(Region region);
        Task Remove(Region region);
    }
}
