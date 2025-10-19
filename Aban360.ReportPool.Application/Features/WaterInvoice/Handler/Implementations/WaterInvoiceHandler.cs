using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.WaterInvoice.Handler.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Features.WaterInvoice.Contracts;
using DNTPersianUtils.Core;
using static Aban360.Common.Timing.CalculationDistanceDate;

namespace Aban360.ReportPool.Application.Features.WaterInvoice.Handler.Implementations
{
    internal sealed class WaterInvoiceHandler : IWaterInvoiceHandler
    {
        private readonly IWaterInvoiceQueryService _waterInvoiceQueryService;
        public WaterInvoiceHandler(IWaterInvoiceQueryService waterInvoiceQueryService)
        {
            _waterInvoiceQueryService = waterInvoiceQueryService;
            _waterInvoiceQueryService.NotNull(nameof(waterInvoiceQueryService));
        }

        public async Task<ReportOutput<WaterInvoiceDto, LineItemsDto>> Handle(string input)
        {
            ReportOutput<WaterInvoiceDto, LineItemsDto> result = await _waterInvoiceQueryService.Get(input);
            result.ReportHeader.ChartIndex = await GetGuageValue(result.ReportHeader.ConsumptionAverage, result.ReportHeader.ContractualCapacity, input, result.ReportHeader.UsageId, result.ReportHeader.ZoneId);
         
            return new ReportOutput<WaterInvoiceDto, LineItemsDto>(result.Title, GetWaterInvoiceData(result.ReportHeader), result.ReportData);
        }
        public WaterInvoiceDto Handle()
        {
            WaterInvoiceDto result = _waterInvoiceQueryService.Get();
            return result;
        }
        private WaterInvoiceDto GetWaterInvoiceData(WaterInvoiceDto input)
        {
            input.BarCode = (input.BillId is null ? new string('0', 13) : input.BillId.PadLeft(13, '0')) +
                            (input.PayId is null ? new string('0', 13) : input.PayId.PadLeft(13, '0'));

            input.PaymenetAmountText = input.PayableAmount.NumberToText(Language.Persian);           

            CalcDistanceResultDto calcDistance = CalcDistance(input.CurrentMeterDateJalali, input.PreviousMeterDateJalali);
            input.Duration = calcDistance.HasError ? 0 : calcDistance.Distance;

            return input;
        }
        private async Task<float> GetGuageValue(float consumptionAverage, int ContractualCapacity, string billId, short usageId, int zoneId)
        {
            if (isDomestic(usageId))
            {
                int olgoo = await _waterInvoiceQueryService.GetOlgo(zoneId);
                if (consumptionAverage <= 5)
                    return 0.5f;
                else if (consumptionAverage <= olgoo)
                    return 1.5f;
                else if (consumptionAverage <= olgoo * 2)
                    return 2.5f;
                else if (consumptionAverage <= olgoo * 4)
                    return 3.5f;
                else
                    return 4f;
            }
            else
            {
                if (consumptionAverage <= (float)ContractualCapacity / 2)
                    return 0.5f;
                else if (consumptionAverage <= ContractualCapacity)
                    return 1.5f;
                else if (consumptionAverage <= ContractualCapacity * 2)
                    return 2.5f;
                else if (consumptionAverage <= ContractualCapacity * 4)
                    return 3.5f;
                else
                    return 4f;
            }

            bool isDomestic(short usageId)
            {
                short[] domesticUsageIds = [1, 3, 25, 34];
                return domesticUsageIds.Contains(usageId);
            }
        }
    }
}
