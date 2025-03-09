using Aban360.MeterPool.Domain.Features.Management.Entities;

namespace Aban360.MeterPool.Persistence.Features.Management.Queries.Contracts
{
    public interface IReadingConfigDefaultQueryService
    {
        Task<ReadingConfigDefault> Get(short id);
        Task<ICollection<ReadingConfigDefault>> Get();
    }
}
