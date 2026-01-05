using Aban360.BlobPool.Domain.Features.DmsServices.Dto.Queries;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Contracts
{
    public interface IGetMetaDataPropertiesHandler
    {
        Task<ICollection<MetaDataOutput>> Handle(string documentId, bool isTitle, CancellationToken cancellationToken);
    }
}
