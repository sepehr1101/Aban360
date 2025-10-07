using Aban360.BlobPool.Domain.Providers.Dto;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts
{
    public interface IAddFileHandler
    {
        Task<AddFileDto> Handle(string billId, string localFilePath, CancellationToken cancellationToken);
    }
}
