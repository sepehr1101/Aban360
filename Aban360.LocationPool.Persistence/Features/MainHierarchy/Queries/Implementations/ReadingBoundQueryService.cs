using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Implementations
{
    public class ReadingBoundQueryService : IReadingBoundQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<ReadingBound> _readingBounds;
        public ReadingBoundQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _readingBounds = _uow.Set<ReadingBound>();
            _readingBounds.NotNull(nameof(_readingBounds));
        }

        public async Task<ReadingBound> Get(int id)
        {
            return await _readingBounds
                    .Include(r => r.Zone)
                    .Where(r => r.Id == id)
                    .SingleAsync();
        }

        public async Task<ICollection<ReadingBound>> Get()
        {
            return await _readingBounds
                .Include(r => r.Zone)
                .ToListAsync();
        }
    }
}
