using Aban360.BlobPool.Domain.Providers.Dto;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Contracts
{
    public interface IGetMetaDataPropertiesHandler
    {
        Task<MetaDataProperties> Handle(string documentId, CancellationToken cancellationToken);
    }
}
