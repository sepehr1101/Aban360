using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;
using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;

namespace Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts
{
    public interface IDocumentQueryService
    {
        Task<Document> Get(Guid id);
        Task<ICollection<Document>> Get(ICollection<Guid> ids, short documentCategoryId);
        Task<ICollection<Document>> Get(ICollection<Guid> ids);
        Task<ICollection<DocumentCategory>> GetDocoumentCategory(ICollection<Guid> ids);
        Task<ICollection<Document>> Get();
    }

}
