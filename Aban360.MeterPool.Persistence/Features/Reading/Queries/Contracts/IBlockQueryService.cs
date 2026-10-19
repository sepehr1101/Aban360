using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;

namespace Aban360.MeterPool.Persistence.Features.Reading.Queries.Contracts
{
    public interface IBlockQueryService
    {
        Task<IEnumerable<BlockGetDto>> Get();
        Task<IEnumerable<BlockGetDto>> Get(Guid id);
    }
}
