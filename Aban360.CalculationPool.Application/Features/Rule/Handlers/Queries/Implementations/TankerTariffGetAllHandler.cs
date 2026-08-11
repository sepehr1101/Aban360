using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Implementations
{
    internal sealed class TankerTariffGetAllHandler : ITankerTariffGetAllHandler
    {
        private readonly ITankerTariffQueryService _queryService;
        public TankerTariffGetAllHandler(ITankerTariffQueryService queryService)
        {
            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<IEnumerable<TankerTariffGetDto>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<TankerTariffGetDto> result = await _queryService.Get();
            return result;
        }
    }
}
