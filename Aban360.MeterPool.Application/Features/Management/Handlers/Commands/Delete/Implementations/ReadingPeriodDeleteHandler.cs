using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Delete.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using Aban360.MeterPool.Persistence.Features.Manegement.Commands.Contracts;
using Aban360.MeterPool.Persistence.Features.Manegement.Queries.Contracts;

namespace Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Delete.Implementations
{
    internal sealed class ReadingPeriodDeleteHandler : IReadingPeriodDeleteHandler
    {
        private readonly IReadingPeriodCommandService _readingPeriodCommandService;
        private readonly IReadingPeriodQueryService _readingPeriodQueryService;
        public ReadingPeriodDeleteHandler(
            IReadingPeriodCommandService readingPeriodCommandService,
            IReadingPeriodQueryService readingPeriodQueryService)
        {
            _readingPeriodCommandService = readingPeriodCommandService;
            _readingPeriodCommandService.NotNull(nameof(_readingPeriodCommandService));

            _readingPeriodQueryService = readingPeriodQueryService;
            _readingPeriodQueryService.NotNull(nameof(_readingPeriodQueryService));
        }

        public async Task Handle(ReadingPeriodDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            ReadingPeriod readingPeriod = await _readingPeriodQueryService.Get(deleteDto.Id);
            _readingPeriodCommandService.Remove(readingPeriod);
        }
    }
}
