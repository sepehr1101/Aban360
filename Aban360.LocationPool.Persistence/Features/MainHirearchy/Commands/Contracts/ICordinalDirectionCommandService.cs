using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;

namespace Aban360.LocationPool.Persistence.Features.MainHirearchy.Commands.Contracts
{
    public interface ICordinalDirectionCommandService
    {
        Task Add(CordinalDirection cordinalDirection);
        Task Remove(CordinalDirection cordinalDirection);
    }
}
