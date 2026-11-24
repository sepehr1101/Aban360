using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;

namespace Aban360.BlobPool.Persistence.Features.Taxonomy.Repositories
{
    internal sealed class ExecutableMimetypeRepository : IExecutableMimetypeCommandService, IExecutableMimetypeQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<ExecutableMimetype> _executableMimetypes;
        public ExecutableMimetypeRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));
            _executableMimetypes = _uow.Set<ExecutableMimetype>();
            _executableMimetypes.NotNull(nameof(_executableMimetypes));
        }
        public async Task Add(ExecutableMimetype executableMimetype)
        {
            await _executableMimetypes.AddAsync(executableMimetype);
        }
        public async Task Add(ICollection<ExecutableMimetype> executableMimetypes)
        {
            await _executableMimetypes.AddRangeAsync(executableMimetypes);
        }
        public void AddSync(ExecutableMimetype executableMimetype)
        {
            _executableMimetypes.Add(executableMimetype);
        }
        public void AddSync(ICollection<ExecutableMimetype> executableMimetypes)
        {
            _executableMimetypes.AddRange(executableMimetypes);
        }
        public void Remove(ExecutableMimetype executableMimetype)
        {
            _executableMimetypes.Remove(executableMimetype);
        }
        public void Remove(ICollection<ExecutableMimetype> executableMimetypes)
        {
            _executableMimetypes.RemoveRange(executableMimetypes);
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
