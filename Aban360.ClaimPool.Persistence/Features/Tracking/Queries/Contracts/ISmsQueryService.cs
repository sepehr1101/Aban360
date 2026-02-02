using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Persistence.Features.Sms.Queries.Contracts
{
    public interface ISmsQueryService
    {
        Task<ReportOutput<TrackingSmsHeaderOutputDto, TrackingSmsDataOutputDto>> Get(Guid trackId);
    }
}
