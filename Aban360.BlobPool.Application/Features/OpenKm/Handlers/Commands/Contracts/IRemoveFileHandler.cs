using Aban360.BlobPool.Domain.Features.OpenKm;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts
{
    public interface IRemoveFileHandler
    {
        Task Handle(RemoveFileDto removeFileDto, CancellationToken cancellationToken);
    }
}