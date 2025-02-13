using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;

namespace Aban360.LocationPool.Persistence.Features.MainHirearchy.Commands.Contracts
{
    public interface IMunicipalityCommandService
    {
        Task Add(Municipality municipality);
        Task Remove(Municipality municipality);
    }
   
}
