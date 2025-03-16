using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Queries.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using Aban360.MeterPool.Persistence.Features.Management.Queries.Contracts;
using AutoMapper;

namespace Aban360.MeterPool.Application.Features.Management.Handlers.Queries.Implementations
{
    internal sealed class CounterStateGetSingleHandler : ICounterStateGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly ICounterStateQueryService _counterStateQueryService;
        public CounterStateGetSingleHandler(
            IMapper mapper,
            ICounterStateQueryService counterStateQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _counterStateQueryService = counterStateQueryService;
            _counterStateQueryService.NotNull(nameof(_counterStateQueryService));
        }

        public async Task<CounterStateGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            CounterState counterState = await _counterStateQueryService.Get(id);
            return _mapper.Map<CounterStateGetDto>(counterState);
        }
    }
}
