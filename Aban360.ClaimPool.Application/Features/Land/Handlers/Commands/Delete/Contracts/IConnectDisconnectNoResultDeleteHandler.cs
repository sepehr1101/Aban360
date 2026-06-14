using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.ApplicationUser;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts
{
    public interface IConnectDisconnectNoResultDeleteHandler
    {
        Task Handle(ConnectDisconnectRemoveInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}
