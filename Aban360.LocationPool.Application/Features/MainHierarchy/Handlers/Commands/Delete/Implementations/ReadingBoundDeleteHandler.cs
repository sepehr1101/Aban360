using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Delete.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Delete.Implementations
{
    internal sealed class ReadingBoundDeleteHandler : IReadingBoundDeleteHandler
    {
        private readonly IReadingBoundQueryService _readingBoundQueryService;
        private readonly IReadingBoundCommandService _readingBoundCommandService;
        public ReadingBoundDeleteHandler(
            IReadingBoundQueryService readingBoundQueryService,
            IReadingBoundCommandService readingBoundCommandService)
        {
            _readingBoundQueryService = readingBoundQueryService;
            _readingBoundQueryService.NotNull(nameof(readingBoundQueryService));

            _readingBoundCommandService = readingBoundCommandService;
            _readingBoundCommandService.NotNull(nameof(readingBoundCommandService));
        }

        public async Task Handle(ReadingBoundDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            ReadingBound readingBound = await _readingBoundQueryService.Get(deleteDto.Id);
            await _readingBoundCommandService.Remove(readingBound);
        }
    }
}
