using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Implementations
{
    public class ReadingBoundGetAllHandler : IReadingBoundGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IReadingBoundQueryService _readingBoundQueryService;
        public ReadingBoundGetAllHandler(
           IMapper mapper,
            IReadingBoundQueryService readingBoundQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _readingBoundQueryService = readingBoundQueryService;
            _readingBoundQueryService.NotNull(nameof(readingBoundQueryService));
        }

        public async Task<ICollection<ReadingBoundGetDto>> Handle(CancellationToken cancellationToken)
        {
            var readingBound = await _readingBoundQueryService.Get();
            return _mapper.Map<ICollection<ReadingBoundGetDto>>(readingBound);
        }
    }
}
