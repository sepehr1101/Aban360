using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Delete.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Delete.Implementations
{
    internal sealed class ReadingBlockDeleteHandler : IReadingBlockDeleteHandler
    {
        private readonly IReadingBlockQeryService _readingBlockQeryService;
        private readonly IReadingBlockCommandService _readingBlockCommandService;
        public ReadingBlockDeleteHandler(
           IReadingBlockQeryService readingBlockQeryService,
            IReadingBlockCommandService readingBlockCommandService)
        {
            _readingBlockQeryService = readingBlockQeryService;
            _readingBlockQeryService.NotNull(nameof(readingBlockQeryService));

            _readingBlockCommandService = readingBlockCommandService;
            _readingBlockCommandService.NotNull(nameof(readingBlockCommandService));
        }

        public async Task Handle(ReadingBlockDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            ReadingBlock readingBlock = await _readingBlockQeryService.Get(deleteDto.Id);
            await _readingBlockCommandService.Remove(readingBlock);
        }
    }
}
