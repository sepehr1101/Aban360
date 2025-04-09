using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;

namespace Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts
{
    public interface IDocumentTypeQueryService
    {
        Task<ICollection<DocumentType>> Get();
        Task<DocumentType> Get(short id);
    }
}