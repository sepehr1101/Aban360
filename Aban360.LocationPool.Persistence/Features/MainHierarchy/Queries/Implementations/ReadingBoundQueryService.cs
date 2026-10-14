using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Implementations
{
    internal sealed class ReadingBoundQueryService : IReadingBoundQueryService
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
            ReadingBound? readingBound= await _readingBounds
                    .Include(r => r.Zone)
                    .Where(r => r.Id == id)
                    .SingleAsync();
            if (readingBound is null)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidId);
            }
            return readingBound;
        }
        public async Task<ICollection<ReadingBound>> GetByZoneId(int zoneId)
        {
            return await _readingBounds
                .Include(r => r.Zone)
                .Where(r => r.ZoneId == zoneId)
                .ToListAsync();
        }

        public async Task<ICollection<ReadingBound>> Get()
        {
            return await _readingBounds
                .Include(r => r.Zone)
                .ToListAsync();
        }
    }
}
