using Aban360.Common.Extensions;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using Aban360.MeterPool.Persistence.Contexts.Contracts;
using Aban360.MeterPool.Persistence.Features.Manegement.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.MeterPool.Persistence.Features.Manegement.Queries.Implementations
{
    public class ReadingPeriodTypeQueryService : IReadingPeriodTypeQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<ReadingPeriodType> _readingPeriodType;
        public ReadingPeriodTypeQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _readingPeriodType = _uow.Set<ReadingPeriodType>();
            _readingPeriodType.NotNull(nameof(_readingPeriodType));
        }

        public async Task<ReadingPeriodType> Get(short id)
        {
            return await _uow.FindOrThrowAsync<ReadingPeriodType>(id);
        }

        public async Task<ICollection<ReadingPeriodType>> Get()
        {
            return await _readingPeriodType.ToListAsync();
        }
    }
}
