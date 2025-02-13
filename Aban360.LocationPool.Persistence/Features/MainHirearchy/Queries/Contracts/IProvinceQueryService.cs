using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;

namespace Aban360.LocationPool.Persistence.Features.MainHirearchy.Queries.Contracts
{
    public interface IProvinceQueryService
    {
        Task<Province> Get(short id);
        Task<ICollection<Province>> Get();
    }
}
