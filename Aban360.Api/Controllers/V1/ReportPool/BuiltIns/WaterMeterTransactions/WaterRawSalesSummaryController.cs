using Aban360.Api.Cronjobs;
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
        private readonly IReportGenerator _reportGenerator;
        public WaterRawSalesSummaryController(
            IWaterRawSalesSummaryHandler waterRawSalesSummary,
            IReportGenerator reportGenerator)
        {
            _waterRawSalesSummary = waterRawSalesSummary;
            _waterRawSalesSummary.NotNull(nameof(_waterRawSalesSummary));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterSalesHeaderOutputDto, WaterRawSalesSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WaterSalesInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<WaterSalesHeaderOutputDto, WaterRawSalesSummaryDataOutputDto> waterSales = await _waterRawSalesSummary.Handle(inputDto, cancellationToken);
            return Ok(waterSales);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, WaterSalesInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _waterRawSalesSummary.Handle, CurrentUser, ReportLiterals.WaterNetSalesSummary, connectionId);
            return Ok(inputDto);
        }
    }
}
