using Aban360.ClaimPool.Domain.Features.Tracking.Dto;

namespace Aban360.ClaimPool.Persistence.Features.Tracking.Queries.Contracts
{
    public interface ITrackingDetailQueryService
    {
        Task<RequestIsRegisterdDto> GetRequestIsRegistered(TrackingDetailGetDto inputDto);
    }
}
