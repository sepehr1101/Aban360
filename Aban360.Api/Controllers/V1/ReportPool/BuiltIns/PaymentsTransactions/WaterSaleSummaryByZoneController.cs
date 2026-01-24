using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.PaymentsTransactions
{
    [Route("v1/water-sale-summary-by-zone")]
    public class WaterSaleSummaryByZoneController : BaseController
    {
        private readonly IWaterSaleSummaryByZoneHandler _waterSaleSummaryByZone;
        private readonly IReportGenerator _reportGenerator;
        public WaterSaleSummaryByZoneController(
            IWaterSaleSummaryByZoneHandler waterSaleSummaryByZone,
            IReportGenerator reportGenerator)
        {
            _waterSaleSummaryByZone = waterSaleSummaryByZone;
            _waterSaleSummaryByZone.NotNull(nameof(_waterSaleSummaryByZone));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterSaleSummaryByZoneHeaderOutputDto, WaterSaleSummaryByZoneDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WaterSaleSummaryByZoneInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<WaterSaleSummaryByZoneHeaderOutputDto, WaterSaleSummaryByZoneDataOutputDto> WaterSaleSummaryByZone = await _waterSaleSummaryByZone.Handle(inputDto, cancellationToken);
            return Ok(WaterSaleSummaryByZone);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, WaterSaleSummaryByZoneInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _waterSaleSummaryByZone.Handle, CurrentUser, ReportLiterals.WaterSaleSummary, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(WaterSaleSummaryByZoneInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 0;
            ReportOutput<WaterSaleSummaryByZoneHeaderOutputDto, WaterSaleSummaryByZoneDataOutputDto> result = await _waterSaleSummaryByZone.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
