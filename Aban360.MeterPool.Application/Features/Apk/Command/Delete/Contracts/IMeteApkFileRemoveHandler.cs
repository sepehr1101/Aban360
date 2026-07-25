using Aban360.Common.ApplicationUser;

namespace Aban360.MeterPool.Application.Features.Apk.Command.Delete.Contracts
{
    public interface IMeteApkFileRemoveHandler
    {
        Task Handle(short id, IAppUser appUser, CancellationToken cancellationToken);
    }
}
