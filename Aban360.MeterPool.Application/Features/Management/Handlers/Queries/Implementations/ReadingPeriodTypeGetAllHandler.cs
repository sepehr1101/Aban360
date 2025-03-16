using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Queries.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using Aban360.MeterPool.Persistence.Features.Manegement.Queries.Contracts;
using AutoMapper;

namespace Aban360.MeterPool.Application.Features.Management.Handlers.Queries.Implementations
{
    internal sealed class ReadingPeriodTypeGetAllHandler : IReadingPeriodTypeGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IReadingPeriodTypeQueryService _readingPeriodTypeQueryService;
        public ReadingPeriodTypeGetAllHandler(
            IMapper mapper,
            IReadingPeriodTypeQueryService readingPeriodTypeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _readingPeriodTypeQueryService = readingPeriodTypeQueryService;
            _readingPeriodTypeQueryService.NotNull(nameof(_readingPeriodTypeQueryService));
        }

        public async Task<ICollection<ReadingPeriodTypeGetDto>> Handle(CancellationToken cancellationToken)
        {
            ICollection<ReadingPeriodType> readingPeriodType = await _readingPeriodTypeQueryService.Get();
            return _mapper.Map<ICollection<ReadingPeriodTypeGetDto>>(readingPeriodType);
        }
    }
}
