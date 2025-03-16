using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Delete.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using Aban360.MeterPool.Persistence.Features.Management.Commands.Contracts;
using Aban360.MeterPool.Persistence.Features.Management.Queries.Contracts;

namespace Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Delete.Implementations
{
    internal sealed class ReadingConfigDefaultDeleteHandler : IReadingConfigDefaultDeleteHandler
    {
        private readonly IReadingConfigDefaultCommandService _readingConfigDefaultCommandService;
        private readonly IReadingConfigDefaultQueryService _readingConfigDefaultQueryService;
        public ReadingConfigDefaultDeleteHandler(
            IReadingConfigDefaultCommandService readingConfigDefaultCommandService,
            IReadingConfigDefaultQueryService readingConfigDefaultQueryService)
        {
            _readingConfigDefaultCommandService = readingConfigDefaultCommandService;
            _readingConfigDefaultCommandService.NotNull(nameof(_readingConfigDefaultCommandService));

            _readingConfigDefaultQueryService = readingConfigDefaultQueryService;
            _readingConfigDefaultQueryService.NotNull(nameof(_readingConfigDefaultQueryService));
        }

        public async Task Handle(ReadingConfigDefaultDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            ReadingConfigDefault readingConfigDefault = await _readingConfigDefaultQueryService.Get(deleteDto.Id);
            await _readingConfigDefaultCommandService.Remove(readingConfigDefault);
        }
    }
}
