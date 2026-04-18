using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts
{
    public interface IKartableRequestGetAllHandler
    {
        Task<ReportOutput<TrackingKartableHeaderOutputDto, TrackingKartableDataOutputDto>> Handle(IAppUser currentUser, CancellationToken cancellationToken);
    }
}
