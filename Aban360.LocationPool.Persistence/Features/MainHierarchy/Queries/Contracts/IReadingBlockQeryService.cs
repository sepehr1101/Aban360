using Aban360.LocationPool.Domain.Features.MainHierarchy;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts
{
    public interface IReadingBlockQeryService
    {
        Task<ReadingBlock> Get(short id);
        Task<ICollection<ReadingBlock>> Get();
    }
}
