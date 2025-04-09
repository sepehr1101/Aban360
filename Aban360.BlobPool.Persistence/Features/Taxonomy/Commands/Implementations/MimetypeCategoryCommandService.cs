using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.BlobPool.Persistence.Features.Taxonomy.Commands.Implementations
{
    internal sealed class MimetypeCategoryCommandService : IMimetypeCategoryCommandService
    {
        private readonly IUnitOfwork _uow;
        private readonly DbSet<MimetypeCategory> _mimetypeCategories;
        public MimetypeCategoryCommandService(IUnitOfwork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _mimetypeCategories = _uow.Set<MimetypeCategory>();
            _mimetypeCategories.NotNull(nameof(_mimetypeCategories));
        }
        public async Task Add(MimetypeCategory mimetypeCategory)
        {
            await _mimetypeCategories.AddAsync(mimetypeCategory);
        }
        public async Task Add(ICollection<MimetypeCategory> mimetypeCategories)
        {
            await _mimetypeCategories.AddRangeAsync(mimetypeCategories);
        }
        public void AddSync(MimetypeCategory mimetypeCategory)
        {
            _mimetypeCategories.Add(mimetypeCategory);
        }
        public void AddSync(ICollection<MimetypeCategory> mimetypeCategories)
        {
            _mimetypeCategories.AddRange(mimetypeCategories);
        }
        public void Remove(MimetypeCategory mimetypeCategory)
        {
            _mimetypeCategories.Remove(mimetypeCategory);
        }
        public void Remove(ICollection<MimetypeCategory> mimetypeCategories)
        {
            _mimetypeCategories.RemoveRange(mimetypeCategories);
        }
    }
}