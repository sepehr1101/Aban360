using Aban360.Common.ApplicationUser;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts
{
    public interface IUsageGroup3RemoveHandler
    {
        Task Handle(short id, IAppUser appUser, CancellationToken cancellationToken);
    }
}
