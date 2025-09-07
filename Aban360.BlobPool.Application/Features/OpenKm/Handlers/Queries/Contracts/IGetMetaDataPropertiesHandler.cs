using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Implementations;
using Aban360.BlobPool.Domain.Features.DmsServices.Dto.Queries;
using Aban360.BlobPool.Domain.Providers.Dto;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Contracts
{
    public interface IGetMetaDataPropertiesHandler
    {
        Task<ICollection<MetaDataOutput>> Handle(string documentId, CancellationToken cancellationToken);
    }
}
