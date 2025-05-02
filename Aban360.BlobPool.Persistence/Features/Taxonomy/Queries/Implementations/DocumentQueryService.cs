using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;
using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Implementations
{
    internal sealed class DocumentQueryService : IDocumentQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Document> _document;
        public DocumentQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _document = _uow.Set<Document>();
            _document.NotNull(nameof(_document));
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
        public async Task<ICollection<DocumentCategoryGetDto>> GetDocoumentCategory(ICollection<Guid> ids)//2
        {
            return await _document
                .Include(d => d.DocumentType)
                .ThenInclude(d => d.DocumentCategory)
                .Where(d => ids.Contains(d.Id))
                .Select(d => new
                {
                    Category = d.DocumentType.DocumentCategory
                })
                .GroupBy(d => d.Category.Id)
                .Select(d => new DocumentCategoryGetDto()
                {
                    Id = d.Key,
                    Title = d.First().Category.Title,
                    Css = d.First().Category.Css,
                    Icon = d.First().Category.Icon,
                })
                .ToListAsync();
        }

        public async Task<ICollection<Document>> Get()
        {
            return await _document.ToListAsync();
        }
    }

}
