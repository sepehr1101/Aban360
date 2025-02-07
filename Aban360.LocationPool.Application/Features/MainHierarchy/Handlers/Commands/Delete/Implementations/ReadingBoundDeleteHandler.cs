using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Delete.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Delete.Implementations
{
    public class ReadingBoundDeleteHandler : IReadingBoundDeleteHandler
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
            var readingBound = await _readingBoundQueryService.Get(deleteDto.Id);
            if (readingBound == null)
            {
                throw new InvalidDataException();
            }
            await _readingBoundCommandService.Remove(readingBound);
        }
    }
}
