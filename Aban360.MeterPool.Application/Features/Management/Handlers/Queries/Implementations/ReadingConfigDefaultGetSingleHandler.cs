using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Queries.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;
using Aban360.MeterPool.Persistence.Features.Management.Queries.Contracts;
using AutoMapper;

namespace Aban360.MeterPool.Application.Features.Management.Handlers.Queries.Implementations
{
    public class ReadingConfigDefaultGetSingleHandler : IReadingConfigDefaultGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IReadingConfigDefaultQueryService _readingConfigDefaultQueryService;
        public ReadingConfigDefaultGetSingleHandler(
            IMapper mapper,
            IReadingConfigDefaultQueryService readingConfigDefaultQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _readingConfigDefaultQueryService = readingConfigDefaultQueryService;
            _readingConfigDefaultQueryService.NotNull(nameof(_readingConfigDefaultQueryService));
        }

        public async Task<ReadingConfigDefaultGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            var readingConfigDefault = await _readingConfigDefaultQueryService.Get(id);
            return _mapper.Map<ReadingConfigDefaultGetDto>(readingConfigDefault);
        }
    }
}
