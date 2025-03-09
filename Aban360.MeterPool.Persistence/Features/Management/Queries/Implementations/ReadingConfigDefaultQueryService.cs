using Aban360.Common.Extensions;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using Aban360.MeterPool.Persistence.Contexts.Contracts;
using Aban360.MeterPool.Persistence.Features.Management.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.MeterPool.Persistence.Features.Management.Queries.Implementations
{
    internal sealed class ReadingConfigDefaultQueryService : IReadingConfigDefaultQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<ReadingConfigDefault> _readingConfigDefault;
        public ReadingConfigDefaultQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _readingConfigDefault = _uow.Set<ReadingConfigDefault>();
            _readingConfigDefault.NotNull(nameof(_readingConfigDefault));
        }

        public async Task<ReadingConfigDefault> Get(short id)
        {
            return await _uow.FindOrThrowAsync<ReadingConfigDefault>(id);
        }

        public async Task<ICollection<ReadingConfigDefault>> Get()
        {
            return await _readingConfigDefault.ToListAsync();
        }
    }
}
