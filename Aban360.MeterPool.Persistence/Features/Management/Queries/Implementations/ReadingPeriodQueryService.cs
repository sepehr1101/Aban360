using Aban360.Common.Extensions;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using Aban360.MeterPool.Persistence.Contexts.Contracts;
using Aban360.MeterPool.Persistence.Features.Manegement.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.MeterPool.Persistence.Features.Manegement.Queries.Implementations
{
    internal sealed class ReadingPeriodQueryService : IReadingPeriodQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<ReadingPeriod> _readingPeriod;
        public ReadingPeriodQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _readingPeriod = _uow.Set<ReadingPeriod>();
            _readingPeriod.NotNull(nameof(_readingPeriod));
        }

        public async Task<ReadingPeriod> Get(short id)
        {
            return await _readingPeriod
                .Include(r => r.ReadingPeriodType)
                .Where(r => r.Id == id)
                .SingleAsync();
        }

        public async Task<ICollection<ReadingPeriod>> Get()
        {
            return await _readingPeriod
                .Include(r => r.ReadingPeriodType)
                .ToListAsync();
        }
    }
}
