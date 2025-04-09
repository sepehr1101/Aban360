using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;

namespace Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts
{
    public interface IDocumentCategoryQueryService
    {
        Task<ICollection<DocumentCategory>> Get();
        Task<DocumentCategory> Get(short id);
    }
}