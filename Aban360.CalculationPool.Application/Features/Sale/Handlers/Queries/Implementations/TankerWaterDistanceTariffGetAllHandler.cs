using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Implementations
{
    internal sealed class TankerWaterDistanceTariffGetAllHandler : ITankerWaterDistanceTariffGetAllHandler
    {
        private readonly ITankerWaterDistanceTariffQueryService _queryService;
        public TankerWaterDistanceTariffGetAllHandler(ITankerWaterDistanceTariffQueryService queryService)
        {
            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<IEnumerable<TankerWaterDistanceTariffOutputDto>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<TankerWaterDistanceTariffOutputDto> result = await _queryService.Get();
            return result;
        }
    }
}
