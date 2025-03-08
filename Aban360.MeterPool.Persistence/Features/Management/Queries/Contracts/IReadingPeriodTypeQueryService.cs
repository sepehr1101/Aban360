using Aban360.MeterPool.Domain.Features.Management.Entities;

namespace Aban360.MeterPool.Persistence.Features.Manegement.Queries.Contracts
{
    public interface IReadingPeriodTypeQueryService
    {
        Task<ReadingPeriodType> Get(short id);
        Task<ICollection<ReadingPeriodType>> Get();
    }
}
