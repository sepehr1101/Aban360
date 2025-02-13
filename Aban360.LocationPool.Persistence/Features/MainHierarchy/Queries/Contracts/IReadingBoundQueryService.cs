using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts
{
    public interface IReadingBoundQueryService
    {
        Task<ReadingBound> Get(int id);
        Task<ICollection<ReadingBound>> Get();
    }
}
