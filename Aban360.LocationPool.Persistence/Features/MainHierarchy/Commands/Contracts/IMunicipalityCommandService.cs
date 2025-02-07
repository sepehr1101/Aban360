using Aban360.LocationPool.Domain.Features.MainHierarchy;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts
{
    public interface IMunicipalityCommandService
    {
        Task Add(Municipality municipality);
        Task Remove(Municipality municipality);
    }
   
}
