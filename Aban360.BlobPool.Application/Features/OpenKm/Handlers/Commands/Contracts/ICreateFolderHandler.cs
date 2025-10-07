using Aban360.BlobPool.Domain.Providers.Dto;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Querys.Contracts
{
    public interface ICreateFolderHandler
    {
        Task<string> Handle(string path, CancellationToken cancellationToken);
    }
}
