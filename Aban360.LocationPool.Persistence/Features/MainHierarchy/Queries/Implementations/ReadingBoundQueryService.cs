using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Implementations
{
    public class ReadingBoundQueryService : IReadingBoundQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<ReadingBound> _readingBound;
        public ReadingBoundQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _readingBound = _uow.Set<ReadingBound>();
            _readingBound.NotNull(nameof(_readingBound));
        }

        public async Task<ReadingBound> Get(int id)
        {
            // return await _uow.FindOrThrowAsync<ReadingBound>(id);
            return await _readingBound
                    .Include(r => r.Zone)
                    .Where(r => r.Id == id)
                    .SingleAsync();
        }

        public async Task<ICollection<ReadingBound>> Get()
        {
            return await _readingBound
                .Include(r => r.Zone)
                .ToListAsync();
        }
    }
}
