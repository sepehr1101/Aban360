using Aban360.BlobPool.Domain.Features.Classification;

namespace Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts
{
    internal interface IDocumentTypeQueryService
    {
        Task<ICollection<DocumentType>> Get();
        Task<DocumentType> Get(int id);
    }
}