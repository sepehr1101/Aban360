using Aban360.Common.Extensions;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Create.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using Aban360.MeterPool.Persistence.Features.Management.Commands.Contracts;
using AutoMapper;

namespace Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Create.Implementations
{
    internal sealed class CounterStateCreateHandler : ICounterStateCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ICounterStateCommandService _counterStateCommandService;
        private readonly IHeadquartersAddhoc _headquarterAddhoc;
        public CounterStateCreateHandler(
            IMapper mapper,
            ICounterStateCommandService counterStateCommandService,
            IHeadquartersAddhoc headquarterAddhoc)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _counterStateCommandService = counterStateCommandService;
            _counterStateCommandService.NotNull(nameof(_counterStateCommandService));

            _headquarterAddhoc = headquarterAddhoc;
            _headquarterAddhoc.NotNull(nameof(_headquarterAddhoc));
        }

        public async Task Handle(CounterStateCreateDto createDto, CancellationToken cancellationToken)
        {
            CounterState counterState = _mapper.Map<CounterState>(createDto);
            string headquarterTitle = await _headquarterAddhoc.Handle(createDto.HeadquartersId, cancellationToken);
            counterState.HeadquartersTitle = headquarterTitle;

            await _counterStateCommandService.Add(counterState);
        }
    }
}
