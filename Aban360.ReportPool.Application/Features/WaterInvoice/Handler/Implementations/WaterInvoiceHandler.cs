using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.WaterInvoice.Handler.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Features.WaterInvoice.Contracts;

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
            return result;
        }
        public WaterInvoiceDto Handle()
        {
            WaterInvoiceDto result =  _waterInvoiceQueryService.Get();
            return result;
        }
    }
}
