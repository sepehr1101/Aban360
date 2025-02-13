using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Implementations
{
    public class ReadingBoundCommandService : IReadingBoundCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<ReadingBound> _readingBound;
        public ReadingBoundCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _readingBound = _uow.Set<ReadingBound>();
            _readingBound.NotNull(nameof(_readingBound));
        }

        public async Task Add(ReadingBound readingBound)
        {
            await _readingBound.AddAsync(readingBound);
        }

        public async Task Remove(ReadingBound readingBound)
        {
            _readingBound.Remove(readingBound);
        }
    }
}
