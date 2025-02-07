using Aban360.LocationPool.Domain.Features.MainHierarchy;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts
{
    public interface IProvinceCommandService
    {
        Task Add(Province province);
        Task Remove(Province province);
    }
}
