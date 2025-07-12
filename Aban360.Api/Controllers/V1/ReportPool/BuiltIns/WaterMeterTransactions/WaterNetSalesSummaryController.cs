using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/water-net-sales-summary")]
    public class WaterNetSalesSummaryController : BaseController
    {
        private readonly IWaterNetSalesSummaryHandler _waterNetSalesSummary;
        public WaterNetSalesSummaryController(IWaterNetSalesSummaryHandler waterNetSalesSummary)
        {
            _waterNetSalesSummary = waterNetSalesSummary;
            _waterNetSalesSummary.NotNull(nameof(_waterNetSalesSummary));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterSalesHeaderOutputDto, WaterNetSalesSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WaterSalesInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<WaterSalesHeaderOutputDto, WaterNetSalesSummaryDataOutputDto> waterSales = await _waterNetSalesSummary.Handle(inputDto, cancellationToken);
            return Ok(waterSales);
        }
    }
}
