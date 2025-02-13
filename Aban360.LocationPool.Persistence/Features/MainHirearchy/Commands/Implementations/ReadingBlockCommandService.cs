using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHirearchy.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.LocationPool.Persistence.Features.MainHirearchy.Commands.Implementations
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
