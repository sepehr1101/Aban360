using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;

namespace Aban360.MeterPool.Persistence.Features.Reading.Queries.Contracts
{
    public interface ITrackingQueryService
    {
        Task<IEnumerable<TrackingGetDto>> Get();
        Task<TrackingGetDto> Get(Guid id);
        Task<TrackingGetDto> Get(long trackNumber);
        Task<IEnumerable<TrackingGetDto>> Get(IEnumerable<long> trackNumbers);
        Task<IEnumerable<TrackingGetDto>> Get(Guid readerId, int flowStepId);
    }
}
