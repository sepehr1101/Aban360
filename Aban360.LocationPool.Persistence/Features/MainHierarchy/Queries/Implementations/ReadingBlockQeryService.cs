using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Implementations
{
    internal sealed class ReadingBlockQeryService : IReadingBlockQeryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<ReadingBlock> _readingBlocks;
        public ReadingBlockQeryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _readingBlocks = _uow.Set<ReadingBlock>();
            _readingBlocks.NotNull(nameof(_readingBlocks));
        }

        public async Task<ReadingBlock> Get(short id)
        {
            return await _readingBlocks
                    .Include(r => r.ReadingBound)
                    .Where(r => r.Id == id)
                    .SingleAsync();
        }

        public async Task<ICollection<ReadingBlock>> Get()
        {
            return await _readingBlocks
                .Include(r=>r.ReadingBound)
                .ToListAsync();
        }
    }
}
