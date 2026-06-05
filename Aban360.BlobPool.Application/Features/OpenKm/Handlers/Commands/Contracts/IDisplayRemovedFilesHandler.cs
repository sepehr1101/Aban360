using Aban360.BlobPool.Domain.Features.OpenKm;
using Aban360.BlobPool.Domain.Providers.Dto;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts
{
    public interface IDisplayRemovedFilesHandler
    {
        Task<FileListResponse> Handle(RemovedFilesInput input, CancellationToken cancellationToken);
    }
}