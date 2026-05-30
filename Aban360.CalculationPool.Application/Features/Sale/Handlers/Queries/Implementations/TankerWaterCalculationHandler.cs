using Aban360.CalculationPool.Application.Features.Base;
using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Implementations
{
    internal sealed class TankerWaterCalculationHandler : ITankerWaterCalculationHandler
    {
        private readonly ITankerWaterDistanceTariffQueryService _tankerQueryService;
        private readonly IZaribCQueryService _zaribCQueryService;
        private readonly IZaribGetService _zaribGetService;
        public TankerWaterCalculationHandler(
            ITankerWaterDistanceTariffQueryService tankerQueryService,
            IZaribCQueryService zaribCQueryService,
            IZaribGetService zaribGetService)
        {
            _tankerQueryService = tankerQueryService;
            _tankerQueryService.NotNull(nameof(tankerQueryService));

            _zaribCQueryService = zaribCQueryService;
            _zaribCQueryService.NotNull(nameof(zaribCQueryService));

            _zaribGetService = zaribGetService;
            _zaribGetService.NotNull(nameof(zaribGetService));
        }
        public async Task<TankerWaterCalculationOutputDto> Handle(TankerWaterCalculationInputDto input, CancellationToken cancellationToken)
        {
            TankerCalculationBaseService tankerService = new TankerCalculationBaseService(_tankerQueryService, _zaribCQueryService, _zaribGetService);
            TankerWaterCalculationOutputDto calcResult = await tankerService.Calculate(input, null);

            return calcResult;
        }
    }
}