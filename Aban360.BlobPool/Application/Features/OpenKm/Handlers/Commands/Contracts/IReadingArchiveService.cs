using Aban360.BlobPool.Domain.Features.DmsServices.Dto.Commands;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.Common.ApplicationUser;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts
{
    public interface IReadingArchiveService
    {
        Task<AddFileDto> Add(AddReadingImageInput input, IAppUser appUser, CancellationToken cancellationToken);
        Task<FileListWithTitlesResponse> Get(string billId, IAppUser appUser, CancellationToken cancellationToken);
    }
}
