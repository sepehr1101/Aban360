using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;

namespace Aban360.BlobPool.Persistence.Features.Taxonomy.Commands.Contracts
{
    public interface IMimetypeCategoryCommandService
    {
        Task Add(ICollection<MimetypeCategory> mimetypeCategories);
        Task Add(MimetypeCategory mimetypeCategory);
        void AddSync(ICollection<MimetypeCategory> mimetypeCategories);
        void AddSync(MimetypeCategory mimetypeCategory);
        void Remove(ICollection<MimetypeCategory> mimetypeCategories);
        void Remove(MimetypeCategory mimetypeCategory);
    }
}