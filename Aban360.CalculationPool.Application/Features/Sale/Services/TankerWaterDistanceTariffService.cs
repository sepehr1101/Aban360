using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.Extensions;
using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Implementations;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;

namespace Aban360.CalculationPool.Application.Features.Sale.Services
{
    public interface ITankerWaterDistanceTariffService
    {
        Task<IEnumerable<TankerWaterDistanceTariffOutputDto>> TankerWaterDistanceTariffGetAllAsync(CancellationToken cancellationToken);
        Task<TankerWaterDistanceTariffOutputDto> TankerWaterDistanceTariffGetAsync(SearchById input, CancellationToken cancellationToken);
    }

    internal sealed class TankerWaterDistanceTariffService : ITankerWaterDistanceTariffService
    {
        private readonly ITankerWaterDistanceTariffQueryService _queryService;
        public TankerWaterDistanceTariffService(ITankerWaterDistanceTariffQueryService queryService)
        {
            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<IEnumerable<TankerWaterDistanceTariffOutputDto>> TankerWaterDistanceTariffGetAllAsync(CancellationToken cancellationToken)
        {
            IEnumerable<TankerWaterDistanceTariffOutputDto> result = await _queryService.Get();
            return result;
        }

        public async Task<TankerWaterDistanceTariffOutputDto> TankerWaterDistanceTariffGetAsync(SearchById input, CancellationToken cancellationToken)
        {
            TankerWaterDistanceTariffOutputDto result = await _queryService.Get(input.Id);
            return result;
        }
    }
}
