using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Update.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using Aban360.MeterPool.Persistence.Features.Manegement.Queries.Contracts;
using AutoMapper;

namespace Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Update.Implementations
{
    internal sealed class ReadingPeriodUpdateHandler : IReadingPeriodUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IReadingPeriodQueryService _readingPeriodQueryService;
        public ReadingPeriodUpdateHandler(
            IMapper mapper,
            IReadingPeriodQueryService readingPeriodQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _readingPeriodQueryService = readingPeriodQueryService;
            _readingPeriodQueryService.NotNull(nameof(_readingPeriodQueryService));
        }

        public async Task Handle(ReadingPeriodUpdateDto updateDto, CancellationToken cancellationToken)
        {
            ReadingPeriod readingPeriod = await _readingPeriodQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, readingPeriod);
        }
    }
}
