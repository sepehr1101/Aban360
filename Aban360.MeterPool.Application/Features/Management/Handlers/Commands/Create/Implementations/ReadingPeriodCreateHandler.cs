using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Create.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using Aban360.MeterPool.Persistence.Features.Manegement.Commands.Contracts;
using AutoMapper;

namespace Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Create.Implementations
{
    public class ReadingPeriodCreateHandler : IReadingPeriodCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IReadingPeriodCommandService _readingPeriodCommandService;
        public ReadingPeriodCreateHandler(
            IMapper mapper,
            IReadingPeriodCommandService readingPeriodCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _readingPeriodCommandService = readingPeriodCommandService;
            _readingPeriodCommandService.NotNull(nameof(_readingPeriodCommandService));
        }

        public async Task Handle(ReadingPeriodCreateDto createDto, CancellationToken cancellationToken)
        {
            var readingPeriod = _mapper.Map<ReadingPeriod>(createDto);
            await _readingPeriodCommandService.Add(readingPeriod);
        }
    }
}
