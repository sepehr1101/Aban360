using Aban360.Common.Extensions;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using Aban360.MeterPool.Persistence.Contexts.Contracts;
using Aban360.MeterPool.Persistence.Features.Manegement.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.MeterPool.Persistence.Features.Manegement.Commands.Implementations
{
    internal sealed class ReadingPeriodCommandService : IReadingPeriodCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<ReadingPeriod> _readingPeriod;
        public ReadingPeriodCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _readingPeriod = _uow.Set<ReadingPeriod>();
            _readingPeriod.NotNull(nameof(_readingPeriod));
        }

        public async Task Add(ReadingPeriod readingPeriod)
        {
            await _readingPeriod.AddAsync(readingPeriod);
        }

        public void Remove(ReadingPeriod readingPeriod)
        {
            _readingPeriod.Remove(readingPeriod);
        }
    }
}