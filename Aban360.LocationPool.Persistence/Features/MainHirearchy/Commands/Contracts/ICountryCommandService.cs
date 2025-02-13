using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;

namespace Aban360.LocationPool.Persistence.Features.MainHirearchy.Commands.Contracts
{
    public interface ICountryCommandService
    {
        Task Add(Country country);
        Task Remove(Country country);
    }
}
