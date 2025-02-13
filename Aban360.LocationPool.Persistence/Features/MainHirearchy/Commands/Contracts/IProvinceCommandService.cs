using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;

namespace Aban360.LocationPool.Persistence.Features.MainHirearchy.Commands.Contracts
{
    public interface IProvinceCommandService
    {
        Task Add(Province province);
        Task Remove(Province province);
    }
}
