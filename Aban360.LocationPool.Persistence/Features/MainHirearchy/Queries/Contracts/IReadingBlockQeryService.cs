using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;

namespace Aban360.LocationPool.Persistence.Features.MainHirearchy.Queries.Contracts
{
    public interface IReadingBlockQeryService
    {
        Task<ReadingBlock> Get(short id);
        Task<ICollection<ReadingBlock>> Get();
    }
}
