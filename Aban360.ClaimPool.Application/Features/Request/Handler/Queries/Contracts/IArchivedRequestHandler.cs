using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts
{
    public interface IArchivedRequestHandler
    {
        Task<ReportOutput<TrackingKartableHeaderOutputDto, TrackingKartableDataOutputDto>> Handle(CancellationToken cancellationToken);
    }
}
