using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Implementations
{
    internal sealed class TankerWaterDistanceTariffGetHandler : ITankerWaterDistanceTariffGetHandler
    {
        private readonly ITankerWaterDistanceTariffQueryService _queryService;
        public TankerWaterDistanceTariffGetHandler(ITankerWaterDistanceTariffQueryService queryService)
        {
            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<TankerWaterDistanceTariffOutputDto> Handle(SearchById input, CancellationToken cancellationToken)
        {
            TankerWaterDistanceTariffOutputDto result = await _queryService.Get(input.Id);
            return result;
        }
    }
}
