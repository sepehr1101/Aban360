using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;

namespace Aban360.MeterPool.Persistence.Features.Reading.Queries.Contracts
{
    public interface ITrackingQueryService
    {
        Task<IEnumerable<TrackingGetDto>> Get();
        Task<IEnumerable<TrackingGetDto>> Get(Guid id);
        Task<IEnumerable<TrackingGetDto>> Get(Guid readerId, int flowStepId);
    }
}
