using Aban360.Common.Extensions;
using Aban360.ReportPool.Persistence.Features.WaterInvoice.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool
{
    [Route("water-invoice")]
    public class WaterInvoiceSummeryInfoController:BaseController
    {
        private readonly IWaterInvoiceQueryService _waterInvoiceQueryService;
        public WaterInvoiceSummeryInfoController(IWaterInvoiceQueryService waterInvoiceQueryService)
        {
            _waterInvoiceQueryService = waterInvoiceQueryService;
            _waterInvoiceQueryService.NotNull(nameof(waterInvoiceQueryService));
        }

        [HttpGet]
        [Route("summery")]
        public IActionResult Get()
        {
            var waterInvoice= _waterInvoiceQueryService.Get();
            return Ok(waterInvoice);
        }
    }
}
