using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;

namespace Aban360.MeterPool.Application.Features.Reading.Handlers.Queries.Contracts
{
    public interface IReadingTrackingFlowGetHandler
    {
        Task<ReportOutput<TrackingHeaderDto, TrackingDataDto>> Handle(int trackNumber, IAppUser appUser, CancellationToken cancellationToken);
    }
}
