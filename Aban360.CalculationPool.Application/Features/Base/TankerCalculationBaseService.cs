using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using DNTPersianUtils.Core;

namespace Aban360.CalculationPool.Application.Features.Base
{
    public sealed class TankerCalculationBaseService
    {
        private readonly ITankerWaterDistanceTariffQueryService _tankerQueryService;
        private readonly IZaribCQueryService _zaribCQueryService;
        private readonly IZaribGetService _zaribGetService;
        static float _hotSeasonMultiple = 0.2f;
        public TankerCalculationBaseService(
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
        public async Task<TankerWaterCalculationOutputDto> Calculate(TankerWaterCalculationInputDto input, string? mobileNumber)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();

            var (c, zb) = await GetZarib(input.ZoneId);
            decimal saleStateZarib = input.SaleState == TankerWaterSaleStateEnum.Nomads ? 1.5m : 4;

            long deliveryAmount = await CalcDeliveryAmount(input);
            decimal abBaha = (input.Consumption * saleStateZarib * c) * zb;
            decimal boodjeh = input.Consumption * 2000m;
            decimal multiplier = GetVarzaneMultiplier(input);


            decimal water = abBaha * multiplier;
            decimal hotSeason = IsHotSeasonDate() ? water * (decimal)_hotSeasonMultiple : 0;

            return new TankerWaterCalculationOutputDto(null, null, null, mobileNumber, water, boodjeh, deliveryAmount, hotSeason);
        }
        private decimal GetVarzaneMultiplier(TankerWaterCalculationInputDto input)
        {
            return input.SaleState != TankerWaterSaleStateEnum.Nomads && input.ZoneId == 133111 ? 0.5m : 1m;
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
        private bool IsHotSeasonDate()
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            string yearJalali = currentDateJalali.Substring(0, 4);

            string hotSeasonFromDateJalali = $"{yearJalali}/03/01";
            string hotSeasonToDateJalali = $"{yearJalali}/06/31";
            return currentDateJalali.CompareTo(hotSeasonFromDateJalali) >= 0 && currentDateJalali.CompareTo(hotSeasonToDateJalali) <= 0;
        }
    }
}
