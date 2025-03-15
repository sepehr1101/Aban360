using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Queries.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using Aban360.MeterPool.Persistence.Features.Management.Queries.Contracts;
using AutoMapper;

namespace Aban360.MeterPool.Application.Features.Management.Handlers.Queries.Implementations
{
    internal sealed class ReadingConfigDefaultGetAllHandler : IReadingConfigDefaultGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IReadingConfigDefaultQueryService _readingConfigDefaultQueryService;
        public ReadingConfigDefaultGetAllHandler(
            IMapper mapper,
            IReadingConfigDefaultQueryService readingConfigDefaultQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _readingConfigDefaultQueryService = readingConfigDefaultQueryService;
            _readingConfigDefaultQueryService.NotNull(nameof(_readingConfigDefaultQueryService));
        }

        public async Task<ICollection<ReadingConfigDefaultGetDto>> Handle(CancellationToken cancellationToken)
        {
            ICollection<ReadingConfigDefault> readingConfigDefault = await _readingConfigDefaultQueryService.Get();
            return _mapper.Map<ICollection<ReadingConfigDefaultGetDto>>(readingConfigDefault);
        }
    }
}
