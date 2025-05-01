using Aban360.BlobPool.Domain.Features.DMS.Entities;

namespace Aban360.BlobPool.Persistence.Features.DMS.Queries.Contracts
{
    public interface IDocumentEntityQueryService
    {
        Task<DocumentEntity> Get(long id);
        Task<ICollection<DocumentEntity>> Get(string id);
        Task<ICollection<DocumentEntity>> Get();
    }
}
