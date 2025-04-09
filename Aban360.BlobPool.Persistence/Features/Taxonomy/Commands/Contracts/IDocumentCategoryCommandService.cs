using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;

namespace Aban360.BlobPool.Persistence.Features.Taxonomy.Commands.Contracts
{
    public interface IDocumentCategoryCommandService
    {
        Task Add(DocumentCategory category);
        Task Add(ICollection<DocumentCategory> documentCategories);
        void AddSync(DocumentCategory documentCategory);
        void AddSync(ICollection<DocumentCategory> documentCategories);
        void Remove(DocumentCategory documentCategory);
        void Remove(ICollection<DocumentCategory> documentCategories);
    }
}