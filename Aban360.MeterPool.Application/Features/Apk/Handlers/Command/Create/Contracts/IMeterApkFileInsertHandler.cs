using Aban360.Common.ApplicationUser;
using Aban360.MeterPool.Domain.Features.Apk.Commands;

namespace Aban360.MeterPool.Application.Features.Apk.Handlers.Command.Create.Contracts
{
    public interface IMeterApkFileInsertHandler
    {
        Task Handle(ApkInfoInsertInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}
