using Aban360.BlobPool.Domain.Providers.Dto;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Implementations
{
    public interface IGetFilesDiscount
    {
        Task<FileListResponse> Handle(string input, CancellationToken cancellationToken);
    }
}