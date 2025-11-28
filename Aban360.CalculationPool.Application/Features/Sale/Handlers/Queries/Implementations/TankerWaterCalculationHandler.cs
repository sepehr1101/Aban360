using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using DNTPersianUtils.Core;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Implementations
{
    internal sealed class TankerWaterCalculationHandler : ITankerWaterCalculationHandler
    {
        private const decimal _vatRate = 0.1m;
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
            long deliveryAmount = 0;
            if (input.Distance > 0)
            {
                TankerWaterDistanceTariffOutputDto tankerTariff = await _tankerQueryService.Get(input.Distance, currentDateJalali);
                deliveryAmount = tankerTariff.Amount * input.Consumption;
            }

            ZaribCQueryDto zaribC = await _zaribCQueryService.GetZaribC(currentDateJalali);
            ZaribGetDto zarib = await _zaribGetService.Get(input.ZoneId, currentDateJalali);

            decimal abBaha = (input.Consumption * zaribC.C) * zarib.Zb;
            decimal boodjeh = input.Consumption * 2000m;

            decimal taxAmount = (abBaha + boodjeh) * _vatRate;
            decimal waterAmountWithoutTax = abBaha + boodjeh;
            decimal waterAmountWithTax = waterAmountWithoutTax + taxAmount;

            decimal finalAmount = Math.Round((waterAmountWithTax + deliveryAmount), 2);

            TankerWaterCalculationOutputDto tankerOuptut = GetTankerWaterCalcOutput(input.ZoneId, taxAmount, waterAmountWithoutTax, deliveryAmount, waterAmountWithTax, finalAmount);

            return tankerOuptut;
        }

        private TankerWaterCalculationOutputDto GetTankerWaterCalcOutput(int zoneId, decimal taxAmount, decimal waterAmountWithoutTax, decimal deliveryAmount, decimal waterAmountWithTax, decimal finalAmount)
        {
            decimal multiplier = zoneId == 133111 ? 0.5m : 1m;

            return new TankerWaterCalculationOutputDto()
            {
                TaxAmount = taxAmount * multiplier,
                WaterAmountWithoutTax = waterAmountWithoutTax * multiplier,
                DeliveryAmount = deliveryAmount * multiplier,
                WaterAmountWithTax = waterAmountWithTax * multiplier,
                FinalAmount = finalAmount * multiplier,
            };
        }
    }
}