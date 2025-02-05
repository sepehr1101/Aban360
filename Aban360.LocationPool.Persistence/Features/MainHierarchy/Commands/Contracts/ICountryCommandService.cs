using Aban360.LocationPool.Domain.Features.MainHierarchy;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts
{
    public interface ICountryCommandService
    {
        Task Add(Country country);
        Task Remove(Country country);
    }
}
