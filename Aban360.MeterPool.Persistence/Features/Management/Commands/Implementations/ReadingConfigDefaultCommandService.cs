using Aban360.Common.Extensions;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using Aban360.MeterPool.Persistence.Contexts.Contracts;
using Aban360.MeterPool.Persistence.Features.Management.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.MeterPool.Persistence.Features.Management.Commands.Implementations
{
    internal sealed class ReadingConfigDefaultCommandService : IReadingConfigDefaultCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<ReadingConfigDefault> _readingConfigDefault;
        public ReadingConfigDefaultCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _readingConfigDefault = _uow.Set<ReadingConfigDefault>();
            _readingConfigDefault.NotNull(nameof(_readingConfigDefault));
        }

        public async Task Add(ReadingConfigDefault readingConfigDefault)
        {
            await _readingConfigDefault.AddAsync(readingConfigDefault);
        }

        public async Task Remove(ReadingConfigDefault readingConfigDefault)
        {
            _readingConfigDefault.Remove(readingConfigDefault);
        }
    }
}
