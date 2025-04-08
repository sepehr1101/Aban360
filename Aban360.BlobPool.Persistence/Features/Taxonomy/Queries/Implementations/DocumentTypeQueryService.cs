using Aban360.BlobPool.Domain.Features.Classification;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Implementations
{
    internal sealed class DocumentTypeQueryService : IDocumentTypeQueryService
    {
        private readonly IUnitOfwork _uow;
        private readonly DbSet<DocumentType> _documentTypes;

        public DocumentTypeQueryService(IUnitOfwork uow)
        {
            _uow = uow;
            _documentTypes = _uow.Set<DocumentType>();
        }

        public async Task<ICollection<DocumentType>> Get()
        {
            return await _documentTypes
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<DocumentType> Get(int id)
        {
            return await _uow
                .FindOrThrowAsync<DocumentType>(id);
        }
    }
}
