using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;

namespace Aban360.LocationPool.Persistence.Features.MainHirearchy.Queries.Contracts
{
    public interface ICordinalDirectionQueryService
    {
        Task<CordinalDirection> Get(short id);
        Task<ICollection<CordinalDirection>> Get();
    }
}
