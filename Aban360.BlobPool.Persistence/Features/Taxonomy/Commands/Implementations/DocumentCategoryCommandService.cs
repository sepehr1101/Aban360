using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.BlobPool.Persistence.Features.Taxonomy.Commands.Implementations
{
    internal sealed class DocumentCategoryCommandService : IDocumentCategoryCommandService
    {
        private readonly IUnitOfwork _uow;
        private readonly DbSet<DocumentCategory> _documentCategories;
        public DocumentCategoryCommandService(IUnitOfwork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _documentCategories = _uow.Set<DocumentCategory>();
            _documentCategories.NotNull(nameof(_documentCategories));
        }
        public async Task Add(DocumentCategory category)
        {
            await _documentCategories.AddAsync(category);
        }
        public void AddSync(DocumentCategory documentCategory)
        {
            _documentCategories.Add(documentCategory);
        }
        public async Task Add(ICollection<DocumentCategory> documentCategories)
        {
            await _documentCategories.AddRangeAsync(documentCategories);
        }
        public void AddSync(ICollection<DocumentCategory> documentCategories)
        {
            _documentCategories.AddRange(documentCategories);
        }
        public void Remove(DocumentCategory documentCategory)
        {
            _documentCategories.Remove(documentCategory);
        }
        public void Remove(ICollection<DocumentCategory> documentCategories)
        {
            _documentCategories.RemoveRange(documentCategories);
        }
    }
}
