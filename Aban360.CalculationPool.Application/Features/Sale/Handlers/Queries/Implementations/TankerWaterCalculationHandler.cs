using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using DNTPersianUtils.Core;

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
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();

            var (c, zb) = await GetZarib(input.ZoneId);
            decimal saleStateZarib = input.SaleState == TankerWaterSaleStateEnum.Nomads ? 1.5m : 4;

            long deliveryAmount = await CalcDeliveryAmount(input);
            decimal abBaha = (input.Consumption * saleStateZarib * c) * zb;
            decimal boodjeh = input.Consumption * 2000m;

            decimal multiplier = input.SaleState != TankerWaterSaleStateEnum.Nomads && input.ZoneId == 133111 ? 0.5m : 1m;

            return new TankerWaterCalculationOutputDto(abBaha * multiplier, boodjeh * multiplier, deliveryAmount);

        }
        private async Task<long> CalcDeliveryAmount(TankerWaterCalculationInputDto input)
        {
            if (input.SaleState != TankerWaterSaleStateEnum.WithTanker)
            {
                return 0;
            }

            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            TankerWaterDistanceTariffOutputDto tankerTariff = await _tankerQueryService.Get(input.Distance, currentDateJalali);
            return tankerTariff.Amount * input.Consumption;
        }
        private async Task<(int, decimal)> GetZarib(int zoneId)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();

            ZaribCQueryDto zaribC = await _zaribCQueryService.GetZaribC(currentDateJalali);
            ZaribGetDto zarib = await _zaribGetService.Get(zoneId, currentDateJalali);

            return (zaribC.C, zarib.Zb);
        }
    }
}