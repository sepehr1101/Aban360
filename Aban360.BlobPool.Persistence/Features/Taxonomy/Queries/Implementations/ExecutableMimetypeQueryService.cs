using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Implementations
{
    internal sealed class ExecutableMimetypeQueryService : IExecutableMimetypeQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<ExecutableMimetype> _executableMimetypes;

        public ExecutableMimetypeQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _executableMimetypes = _uow.Set<ExecutableMimetype>();
        }

        public async Task<ICollection<ExecutableMimetype>> Get()
        {
            return await _executableMimetypes
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ExecutableMimetype> Get(short id)
        {
            return await _uow
                .FindOrThrowAsync<ExecutableMimetype>(id);
        }
    }
}