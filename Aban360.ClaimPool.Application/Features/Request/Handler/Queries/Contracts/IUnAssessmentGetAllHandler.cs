using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts
{
    public interface IUnAssessmentGetAllHandler
    {
        Task<ReportOutput<UnAssessmentHeaderOutputDto, UnAssessmentDataOutputDto>> Handle(IAppUser appUser, CancellationToken cancellationToken);
    }
}
