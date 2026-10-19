using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;

namespace Aban360.MeterPool.Persistence.Features.Reading.Queries.Contracts
{
    public interface IDetailQueryService
    {
        Task<IEnumerable<DetailGetDto>> Get();
        Task<IEnumerable<DetailGetDto>> Get(Guid id);
        Task<IEnumerable<DetailGetDto>> GetByTrackIds(IEnumerable<Guid> ids);
    }
}
