using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Implementations
{
    public class ReadingBlockCommandService : IReadingBlockCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<ReadingBlock> _readingBlock;
        public ReadingBlockCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _readingBlock = _uow.Set<ReadingBlock>();
            _readingBlock.NotNull(nameof(_readingBlock));
        }

        public async Task Add(ReadingBlock readingBlock)
        {
            await _readingBlock.AddRangeAsync(readingBlock);
        }

        public async Task Remove(ReadingBlock readingBlock)
        {
            _readingBlock.Remove(readingBlock);
        }
    }
}
