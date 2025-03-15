using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Implementations
{
    internal sealed class ReadingBoundGetSingleHandler : IReadingBoundGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IReadingBoundQueryService _readingBoundQueryService;
        public ReadingBoundGetSingleHandler(
           IMapper mapper,
            IReadingBoundQueryService readingBoundQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _readingBoundQueryService = readingBoundQueryService;
            _readingBoundQueryService.NotNull(nameof(readingBoundQueryService));
        }

        public async Task<ReadingBoundGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            ReadingBound readingBound = await _readingBoundQueryService.Get(id);
            return _mapper.Map<ReadingBoundGetDto>(readingBound);
        }
    }
}
