using Aban360.Common.ApplicationUser;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts
{
    public interface IConCompanyRemoveHandler
    {
        Task Handle(int id, IAppUser appUser, CancellationToken cancellationToken);
    }
}
