using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts
{
    public interface ICordinalDirectionCommandService
    {
        Task Add(CordinalDirection cordinalDirection);
        Task Remove(CordinalDirection cordinalDirection);
    }
}
