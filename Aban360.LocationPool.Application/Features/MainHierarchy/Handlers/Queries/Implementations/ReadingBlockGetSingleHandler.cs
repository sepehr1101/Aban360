using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Implementations
{
    internal sealed class ReadingBlockGetSingleHandler : IReadingBlockGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IReadingBlockQeryService _readingBlockQeryService;
        public ReadingBlockGetSingleHandler(
            IMapper mapper,
            IReadingBlockQeryService readingBlockQeryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _readingBlockQeryService = readingBlockQeryService;
            _readingBlockQeryService.NotNull(nameof(readingBlockQeryService));
        }

        public async Task<ReadingBlockGetDto> Handle(short id,CancellationToken cancellationToken)
        {
            ReadingBlock readingBound = await _readingBlockQeryService.Get(id);
            return _mapper.Map<ReadingBlockGetDto>(readingBound);
        }
    }
}
