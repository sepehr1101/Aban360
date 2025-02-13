using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Queries;
using Aban360.LocationPool.Persistence.Features.MainHirearchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Queries.Implementations
{
    public class ReadingBoundGetSingleHandler : IReadingBoundGetSingleHandler
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

        public async Task<ReadingBoundGetDto> Handle(short id,CancellationToken cancellationToken)
        {
            var readingBound = await _readingBoundQueryService.Get();
            return _mapper.Map<ReadingBoundGetDto>(readingBound);
        }
    }
}
