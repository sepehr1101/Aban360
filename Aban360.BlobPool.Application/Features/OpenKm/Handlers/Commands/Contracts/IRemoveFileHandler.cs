using Aban360.BlobPool.Domain.Features.OpenKm;
using Aban360.Common.ApplicationUser;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts
{
    public interface IRemoveFileHandler
    {
        Task Handle(RemoveFileDto removeFileDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}