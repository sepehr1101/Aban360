using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Delete.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using Aban360.MeterPool.Persistence.Features.Manegement.Commands.Contracts;
using Aban360.MeterPool.Persistence.Features.Manegement.Queries.Contracts;

namespace Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Delete.Implementations
{
    internal sealed class ReadingPeriodTypeDeleteHandler : IReadingPeriodTypeDeleteHandler
    {
        private readonly IReadingPeriodTypeCommandService _readingPeriodTypeCommandService;
        private readonly IReadingPeriodTypeQueryService _readingPeriodTypeQueryService;
        public ReadingPeriodTypeDeleteHandler(
            IReadingPeriodTypeCommandService readingPeriodTypeCommandService,
            IReadingPeriodTypeQueryService readingPeriodTypeQueryService)
        {
            _readingPeriodTypeCommandService = readingPeriodTypeCommandService;
            _readingPeriodTypeCommandService.NotNull(nameof(_readingPeriodTypeCommandService));

            _readingPeriodTypeQueryService = readingPeriodTypeQueryService;
            _readingPeriodTypeQueryService.NotNull(nameof(_readingPeriodTypeQueryService));
        }

        public async Task Handle(ReadingPeriodTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            ReadingPeriodType readingPeriodType = await _readingPeriodTypeQueryService.Get(deleteDto.Id);            
            _readingPeriodTypeCommandService.Remove(readingPeriodType);
        }
    }
}