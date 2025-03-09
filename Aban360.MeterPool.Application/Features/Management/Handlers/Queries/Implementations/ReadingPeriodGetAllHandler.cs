using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Queries.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;
using Aban360.MeterPool.Persistence.Features.Manegement.Queries.Contracts;
using AutoMapper;

namespace Aban360.MeterPool.Application.Features.Management.Handlers.Queries.Implementations
{
    public class ReadingPeriodGetAllHandler : IReadingPeriodGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IReadingPeriodQueryService _readingPeriodQueryService;
        public ReadingPeriodGetAllHandler(
            IMapper mapper,
            IReadingPeriodQueryService readingPeriodQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _readingPeriodQueryService = readingPeriodQueryService;
            _readingPeriodQueryService.NotNull(nameof(_readingPeriodQueryService));
        }

        public async Task<ICollection<ReadingPeriodGetDto>> Handle(CancellationToken cancellationToken)
        {
            var readingPeriod = await _readingPeriodQueryService.Get();
            if (readingPeriod == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<ICollection<ReadingPeriodGetDto>>(readingPeriod);
        }
    }
}
