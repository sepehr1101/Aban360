using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
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
        private readonly IReportGenerator _reportGenerator;
        public WaterNetSalesSummaryController(
            IWaterNetSalesSummaryHandler waterNetSalesSummary,
            IReportGenerator reportGenerator)
        {
            _waterNetSalesSummary = waterNetSalesSummary;
            _waterNetSalesSummary.NotNull(nameof(_waterNetSalesSummary));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterSalesHeaderOutputDto, WaterNetSalesSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WaterSalesInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<WaterSalesHeaderOutputDto, WaterNetSalesSummaryDataOutputDto> waterSales = await _waterNetSalesSummary.Handle(inputDto, cancellationToken);
            return Ok(waterSales);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, WaterSalesInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _waterNetSalesSummary.Handle, CurrentUser, ReportLiterals.WaterNetSalesSummary , connectionId);
            return Ok(inputDto);
        }
    }
}
