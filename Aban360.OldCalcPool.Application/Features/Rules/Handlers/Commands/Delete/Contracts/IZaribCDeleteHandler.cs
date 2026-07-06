using Aban360.Common.ApplicationUser;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Delete.Contracts
{
    public interface IZaribCDeleteHandler
    {
        Task Handle(int id, IAppUser appUser, CancellationToken cancellationToken);
    }
}
