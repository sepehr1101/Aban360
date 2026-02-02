using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Application.Features.Sms.Handler.Queries.Contracts
{
    public interface ISmsByTrackIdGetHandler
    {
        Task<ReportOutput<TrackingSmsHeaderOutputDto, TrackingSmsDataOutputDto>> Handle(Guid trackId, CancellationToken cancellationToken);
    }
}
