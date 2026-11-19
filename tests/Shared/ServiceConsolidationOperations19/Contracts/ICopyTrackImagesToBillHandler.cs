using Aban360.BlobPool.Domain.Features.OpenKm;
using Aban360.Common.ApplicationUser;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts
{
    public interface ICopyTrackImagesToBillHandler
    {
        Task<CopyTrackImagesToBillOutputDto> Handle(CopyTrackImagesToBillInputDto input, IAppUser appUser, CancellationToken cancellationToken);
    }
}
