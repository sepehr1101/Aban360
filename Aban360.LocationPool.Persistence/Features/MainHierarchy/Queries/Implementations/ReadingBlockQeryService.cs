using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Implementations
{
    public class ReadingBlockQeryService : IReadingBlockQeryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<ReadingBlock> _readingBlock;
        public ReadingBlockQeryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _readingBlock = _uow.Set<ReadingBlock>();
            _readingBlock.NotNull(nameof(_readingBlock));
        }

        public async Task<ReadingBlock> Get(short id)
        {
            //return await _uow.FindOrThrowAsync<ReadingBlock>(id);
            return await _readingBlock
                    .Include(r => r.ReadingBoundId)
                    .Where(r => r.Id == id)
                    .SingleAsync();
        }

        public async Task<ICollection<ReadingBlock>> Get()
        {
            return await _readingBlock
                .Include(r=>r.ReadingBoundId)
                .ToListAsync();
        }
    }
}
