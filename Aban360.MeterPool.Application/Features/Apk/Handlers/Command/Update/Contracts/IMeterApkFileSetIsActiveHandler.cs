using Aban360.Common.ApplicationUser;

namespace Aban360.MeterPool.Application.Features.Apk.Handlers.Command.Update.Contracts
{
    public interface IMeterApkFileSetIsActiveHandler
    {
        Task Handle(int id, IAppUser appUser, CancellationToken cancellationToken);
    }
}
