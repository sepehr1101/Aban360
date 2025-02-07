using Aban360.LocationPool.Domain.Features.MainHierarchy;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts
{
    public interface ICordinalDirectionQueryService
    {
        Task<CordinalDirection> Get(short id);
        Task<ICollection<CordinalDirection>> Get();
    }
}
