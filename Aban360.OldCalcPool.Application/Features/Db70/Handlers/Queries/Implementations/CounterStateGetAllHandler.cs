using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts;

namespace Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Implementations
{
    internal sealed class CounterStateGetAllHandler : ICounterStateGetAllHandler
    {
        private readonly ICounterStateQueryService _CounterStateQueryService;

        public CounterStateGetAllHandler(ICounterStateQueryService CounterStateQueryService)
        {
            _CounterStateQueryService = CounterStateQueryService;
            _CounterStateQueryService.NotNull(nameof(CounterStateQueryService));
        }
        public async Task<IEnumerable<CounterStateCodeDto>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<CounterStateCodeDto> result = await _CounterStateQueryService.Get();
            return result;
        }
    }
}
