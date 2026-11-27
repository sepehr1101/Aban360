using Aban360.MeterPool.Domain.Features.Management.Entities;

namespace Aban360.MeterPool.Persistence.Features.Manegement.Queries.Contracts
{
    public interface IReadingPeriodQueryService
    {
        Task<ReadingPeriod> Get(short id);
        Task<ICollection<ReadingPeriod>> Get();
    }
}
