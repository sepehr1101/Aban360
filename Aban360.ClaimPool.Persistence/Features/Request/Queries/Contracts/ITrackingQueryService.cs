using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts
{
    public interface ITrackingQueryService
    {
        Task<TrackingOutputDto> GetFirstStep(int trackNumber);
        Task<TrackingOutputDto> GetLatest(int trackNumber);
        Task<IEnumerable<TrackingKartableDataOutputDto>> GetAllOpenRequest();
    }
}
