using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;

namespace Aban360.BlobPool.Persistence.Features.Taxonomy.Repositories
{
    internal sealed class DocumentRepository : IDocumentCommandService, IDocumentQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Document> _document;
        public DocumentRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));
            _document = _uow.Set<Document>();
            _document.NotNull(nameof(_document));
        }

        public async Task Add(Document document)
        {
            await _document.AddAsync(document);
        }

        public async Task Remove(Document document)
        {
            _document.Remove(document);
        }

        public async Task<Document> Get(Guid id)
        {
            return await _uow.FindOrThrowAsync<Document>(id);
        }
        public async Task<ICollection<Document>> Get(ICollection<Guid> ids, short documentCategoryId)
        {
            return await _document
                .Include(d => d.DocumentType)
                .ThenInclude(d => d.DocumentCategory)
                .Where(d => ids.Contains(d.Id) & d.DocumentType.DocumentCategory.Id == documentCategoryId)
                .ToListAsync();
        }
        public async Task<ICollection<Document>> Get(ICollection<Guid> ids)//1
        {
            return await _document
                 .Include(d => d.DocumentType)
                 .ThenInclude(d => d.DocumentCategory)
                 .Where(d => ids.Contains(d.Id))
                 .ToListAsync();
        }
        public async Task<ICollection<DocumentCategory>> GetDocoumentCategory(ICollection<Guid> ids)
        {
            return await _document
                   .Include(d => d.DocumentType)
                   .ThenInclude(d => d.DocumentCategory)
                   .Where(d => ids.Contains(d.Id))
                   .Select(d => d.DocumentType.DocumentCategory)
                   .Distinct()
                   .ToListAsync();
        }

        public async Task<ICollection<Document>> Get()
        {
            return await _document.ToListAsync();
        }
    }
}
