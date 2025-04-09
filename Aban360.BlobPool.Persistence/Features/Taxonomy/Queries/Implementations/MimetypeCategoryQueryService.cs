using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Implementations
{
    internal sealed class MimetypeCategoryQueryService : IMimetypeCategoryQueryService
    {
        private readonly IUnitOfwork _uow;
        private readonly DbSet<MimetypeCategory> _mimetypeCategories;

        public MimetypeCategoryQueryService(IUnitOfwork uow)
        {
            _uow = uow;
            _mimetypeCategories = _uow.Set<MimetypeCategory>();
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
