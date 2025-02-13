using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;

namespace Aban360.LocationPool.Persistence.Features.MainHirearchy.Commands.Contracts
{
    public interface IHeadquarterCommandService
    {
        Task Add(Headquarters headquarter);
        Task Remove(Headquarters headquarter);
    }
}
