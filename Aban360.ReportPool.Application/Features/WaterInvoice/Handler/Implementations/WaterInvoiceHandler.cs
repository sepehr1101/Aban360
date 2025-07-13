using Aban360.Common.Extensions;
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

        public async Task<WaterInvoiceDto> Handle(string input)
        {
            WaterInvoiceDto result = await _waterInvoiceQueryService.Get(input);
            result.BarCode = (result.BillId is null ? new string('0', 13) : result.BillId.PadLeft(13, '0')) +
                             (result.PayId is null ? new string('0', 13) : result.PayId.PadLeft(13, '0'));

            result.PaymenetAmountText= result.PayableAmount.NumberToText(Language.Persian);

            return result;
        }
        public WaterInvoiceDto Handle()
        {
            WaterInvoiceDto result =  _waterInvoiceQueryService.Get();
            return result;
        }
    }
}
