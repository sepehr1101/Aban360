using Aban360.BlobPool.Domain.Features.Classification;

namespace Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts
{
    internal interface IDocumentCategoryQueryService
    {
        Task<ICollection<DocumentCategory>> Get();
        Task<DocumentCategory> Get(int id);
    }
}