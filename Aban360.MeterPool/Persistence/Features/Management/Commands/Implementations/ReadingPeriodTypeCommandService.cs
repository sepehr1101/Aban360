using Aban360.Common.Extensions;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using Aban360.MeterPool.Persistence.Contexts.Contracts;
using Aban360.MeterPool.Persistence.Features.Manegement.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.MeterPool.Persistence.Features.Manegement.Commands.Implementations
{
    internal sealed class ReadingPeriodTypeCommandService : IReadingPeriodTypeCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<ReadingPeriodType> _readingPeriodType;
        public ReadingPeriodTypeCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _readingPeriodType = _uow.Set<ReadingPeriodType>();
            _readingPeriodType.NotNull(nameof(_readingPeriodType));
        }

        public async Task Add(ReadingPeriodType readingPeriodType)
        {
            await _readingPeriodType.AddAsync(readingPeriodType);
        }

        public void Remove(ReadingPeriodType readingPeriodType)
        {
            _readingPeriodType.Remove(readingPeriodType);
        }
    }
}
