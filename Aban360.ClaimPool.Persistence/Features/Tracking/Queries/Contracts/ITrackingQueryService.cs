using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Persistence.Features.Tracking.Queries.Contracts
{
    public interface ITrackingQueryService
    {
        Task<ReportOutput<TrackingDisplayFlowHeaderOutputDto, TrackingDisplayFlowDateOutputDto>> Get(int trackNumber);
    }
}
