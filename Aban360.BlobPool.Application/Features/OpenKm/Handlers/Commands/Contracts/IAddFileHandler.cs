using Aban360.BlobPool.Domain.Features.DmsServices.Dto.Commands;
using Aban360.BlobPool.Domain.Providers.Dto;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts
{
    public interface IAddFileHandler
    {
        Task<AddFileDto> Handle(string billId, string localFilePath, CancellationToken cancellationToken);
        Task<AddFileDto> Handle(AddFormFileInput input, CancellationToken cancellationToken);
        Task<AddFileDto> Handle(AddBase64FileInput input, CancellationToken cancellationToken);
    }
}
