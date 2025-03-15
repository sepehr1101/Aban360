using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Queries.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using Aban360.MeterPool.Persistence.Features.Manegement.Queries.Contracts;
using AutoMapper;

namespace Aban360.MeterPool.Application.Features.Management.Handlers.Queries.Implementations
{
    internal sealed class ReadingPeriodGetSingleHandler : IReadingPeriodGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IReadingPeriodQueryService _readingPeriodQueryService;
        public ReadingPeriodGetSingleHandler(
            IMapper mapper,
            IReadingPeriodQueryService readingPeriodQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _readingPeriodQueryService = readingPeriodQueryService;
            _readingPeriodQueryService.NotNull(nameof(_readingPeriodQueryService));
        }

        public async Task<ReadingPeriodGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            ReadingPeriod readingPeriod = await _readingPeriodQueryService.Get(id);
            return _mapper.Map<ReadingPeriodGetDto>(readingPeriod);
        }
    }
}
