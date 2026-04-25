using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts
{
    public interface ITrackingQueryService
    {
        Task<TrackingOutputDto?> GetFirstStep(int trackNumber, bool hasException = true);
        Task<TrackingOutputDto> Get(Guid trackId);
        Task<TrackingOutputDto> GetLatest(int trackNumber, bool hasException = true);
        Task<TrackingOutputDto> GetSecondToLatest(int trackNumber);
        Task<IEnumerable<TrackingKartableDataOutputDto>> GetAllOpenRequest(IEnumerable<int> zoneIds);
        Task<IEnumerable<TrackingKartableDataOutputDto>> GetAllArchivedRequest();
        Task<IEnumerable<UnconfirmedRequestDataOutputDto>> GetUnconfirmedRequestByZoneId(int zoneId);
    }
}
