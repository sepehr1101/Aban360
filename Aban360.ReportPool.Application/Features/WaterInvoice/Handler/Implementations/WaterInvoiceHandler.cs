using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.Common.Timing;
using Aban360.ReportPool.Application.Features.WaterInvoice.Handler.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Features.WaterInvoice.Contracts;
using DNTPersianUtils.Core;

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
            input.Duration = int.Parse(CalculationDistanceDate.CalcDistance(input.CurrentMeterDateJalali, input.PreviousMeterDateJalali));

            //var currentMeterDate = input.CurrentMeterDateJalali.ToGregorianDateOnly();
            //var previousMeterDate = input.PreviousMeterDateJalali.ToGregorianDateOnly();
            //if (!currentMeterDate.HasValue || !previousMeterDate.HasValue)
            //    input.Duration = 0;
            //else
            //    input.Duration = (currentMeterDate.Value.DayNumber) - (previousMeterDate.Value.DayNumber);

            return input;
        }
    }
}
