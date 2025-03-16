using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Delete.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using Aban360.MeterPool.Persistence.Features.Management.Commands.Contracts;
using Aban360.MeterPool.Persistence.Features.Management.Queries.Contracts;

namespace Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Delete.Implementations
{
    internal sealed class CounterStateDeleteHandler : ICounterStateDeleteHandler
    {
        private readonly ICounterStateCommandService _counterStateCommandService;
        private readonly ICounterStateQueryService _counterStateQueryService;
        public CounterStateDeleteHandler(
            ICounterStateCommandService counterStateCommandService,
            ICounterStateQueryService CounterStateQueryService)
        {
            _counterStateCommandService = counterStateCommandService;
            _counterStateCommandService.NotNull(nameof(_counterStateCommandService));

            _counterStateQueryService = CounterStateQueryService;
            _counterStateQueryService.NotNull(nameof(_counterStateQueryService));
        }

        public async Task Handle(CounterStateDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            CounterState counterState = await _counterStateQueryService.Get(deleteDto.Id);
            await _counterStateCommandService.Remove(counterState);
        }
    }
}
