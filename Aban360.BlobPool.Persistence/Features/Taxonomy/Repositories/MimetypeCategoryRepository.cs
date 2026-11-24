using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;

namespace Aban360.BlobPool.Persistence.Features.Taxonomy.Repositories
{
    internal sealed class MimetypeCategoryRepository : IMimetypeCategoryCommandService, IMimetypeCategoryQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<MimetypeCategory> _mimetypeCategories;
        public MimetypeCategoryRepository(IUnitOfWork uow)
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

        public async Task<ICollection<MimetypeCategory>> Get()
        {
            return await _mimetypeCategories
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<MimetypeCategory> Get(short id)
        {
            return await _uow
                .FindOrThrowAsync<MimetypeCategory>(id);
        }
    }
}
