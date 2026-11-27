using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts
{
    public interface IProvinceQueryService
    {
        Task<Province> Get(short id);
        Task<ICollection<Province>> Get();
    }
}
