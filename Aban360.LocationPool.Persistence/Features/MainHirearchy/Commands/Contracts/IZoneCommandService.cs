using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;

namespace Aban360.LocationPool.Persistence.Features.MainHirearchy.Commands.Contracts
{
    public interface IZoneCommandService
    {
        Task Add(Zone zone);
        Task Remove(Zone zone);
    }
}
