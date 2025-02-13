using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;

namespace Aban360.LocationPool.Persistence.Features.MainHirearchy.Queries.Contracts
{
    public interface IReadingBoundQueryService
    {
        Task<ReadingBound> Get(int id);
        Task<ICollection<ReadingBound>> Get();
    }
}
