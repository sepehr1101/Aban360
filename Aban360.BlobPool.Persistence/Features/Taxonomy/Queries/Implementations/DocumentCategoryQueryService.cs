using Aban360.BlobPool.Domain.Features.Classification;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Implementations
{
    internal sealed class DocumentCategoryQueryService : IDocumentCategoryQueryService
    {
        private readonly IUnitOfwork _uow;
        private readonly DbSet<DocumentCategory> _documentCategories;
        public DocumentCategoryQueryService(IUnitOfwork uow)
        {
            _uow = uow;
            _documentCategories = _uow.Set<DocumentCategory>();
        }
        public async Task<ICollection<DocumentCategory>> Get()
        {
            return await _documentCategories
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<DocumentCategory> Get(int id)
        {
            return await _uow
                .FindOrThrowAsync<DocumentCategory>(id);
        }
    }
}
