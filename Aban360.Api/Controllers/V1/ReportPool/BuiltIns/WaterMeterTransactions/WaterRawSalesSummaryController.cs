using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/water-raw-sales-summary")]
    public class WaterRawSalesSummaryController : BaseController
    {
        private readonly IWaterRawSalesSummaryHandler _waterRawSalesSummary;
        public WaterRawSalesSummaryController(IWaterRawSalesSummaryHandler waterRawSalesSummary)
        {
            _waterRawSalesSummary = waterRawSalesSummary;
            _waterRawSalesSummary.NotNull(nameof(_waterRawSalesSummary));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterSalesHeaderOutputDto, WaterRawSalesSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WaterSalesInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<WaterSalesHeaderOutputDto, WaterRawSalesSummaryDataOutputDto> waterSales = await _waterRawSalesSummary.Handle(inputDto, cancellationToken);
            return Ok(waterSales);
        }
    }
}
