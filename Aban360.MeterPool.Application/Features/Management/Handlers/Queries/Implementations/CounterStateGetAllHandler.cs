using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Queries.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;
using Aban360.MeterPool.Persistence.Features.Management.Queries.Contracts;
using AutoMapper;

namespace Aban360.MeterPool.Application.Features.Management.Handlers.Queries.Implementations
{
    public class CounterStateGetAllHandler : ICounterStateGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly ICounterStateQueryService _counterStateQueryService;
        public CounterStateGetAllHandler(
            IMapper mapper,
            ICounterStateQueryService counterStateQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _counterStateQueryService = counterStateQueryService;
            _counterStateQueryService.NotNull(nameof(_counterStateQueryService));
        }

        public async Task<ICollection<CounterStateGetDto>> Handle(CancellationToken cancellationToken)
        {
            var counterState = await _counterStateQueryService.Get();
            return _mapper.Map<ICollection<CounterStateGetDto>>(counterState);
        }
    }
}
