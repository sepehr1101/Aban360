using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;

namespace Aban360.MeterPool.Persistence.Features.Reading.Queries.Contracts
{
    public interface IReadingCounterStateQueryService
    {
        Task<IEnumerable<ReadingCounterStateGetDto>> Get();
        Task<ReadingCounterStateGetDto> Get(int id, bool hasException);
    }
}
