using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.ApplicationUser;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts
{
    public interface IRequestBasicInfoGetHandler
    {
        Task<RequestBasicInfoDataOutputDto> Handle(int trackNumber, IAppUser appUser, CancellationToken cancellationToken);
    }
}
