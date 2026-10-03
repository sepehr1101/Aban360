using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.Common.ApplicationUser;

namespace Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Contracts
{
    public interface ISetExaminationResultDetailHandler
    {
        Task<SetExaminationResultOutputDto> Handle(Guid id, IAppUser appUser, CancellationToken cancellationToken);
    }

}
