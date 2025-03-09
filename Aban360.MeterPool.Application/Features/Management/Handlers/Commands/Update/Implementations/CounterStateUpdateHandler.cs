using Aban360.Common.Extensions;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Update.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Persistence.Features.Management.Queries.Contracts;
using AutoMapper;

namespace Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Update.Implementations
{
    public class CounterStateUpdateHandler : ICounterStateUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ICounterStateQueryService _counterStateQueryService;
        private readonly IHeadquartersAddhoc _headquarterAddhoc;
        public CounterStateUpdateHandler(
            IMapper mapper,
            ICounterStateQueryService counterStateQueryService,
            IHeadquartersAddhoc headquarterAddhoc)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _counterStateQueryService = counterStateQueryService;
            _counterStateQueryService.NotNull(nameof(_counterStateQueryService));

            _headquarterAddhoc = headquarterAddhoc;
            _headquarterAddhoc.NotNull(nameof(_headquarterAddhoc));
        }

        public async Task Handle(CounterStateUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var counterStateDto = await _counterStateQueryService.Get(updateDto.Id);

            var headquarterTitle = await _headquarterAddhoc.Handle(updateDto.HeadquartersId, cancellationToken);
            counterStateDto.HeadquartersTitle = headquarterTitle;

            _mapper.Map(counterStateDto, updateDto);
        }
    }
}
