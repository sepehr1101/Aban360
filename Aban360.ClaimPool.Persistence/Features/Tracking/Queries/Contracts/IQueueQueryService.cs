using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Persistence.Features.Sms.Queries.Contracts
{
    public interface IQueueQueryService
    {
        Task<ReportOutput<TrackingSmsHeaderOutputDto, TrackingSmsDataOutputDto>> GetByTrackId(Guid trackId);
        Task<TrackingSmsDataOutputDto> Get(Guid id);
    }
}
