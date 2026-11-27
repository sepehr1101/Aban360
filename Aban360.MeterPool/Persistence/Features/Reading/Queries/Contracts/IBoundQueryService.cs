using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;

namespace Aban360.MeterPool.Persistence.Features.Reading.Queries.Contracts
{
    public interface IBoundQueryService
    {
        Task<IEnumerable<BoundGetDto>> Get();
        Task<IEnumerable<BoundGetDto>> Get(Guid id);
    }
}
