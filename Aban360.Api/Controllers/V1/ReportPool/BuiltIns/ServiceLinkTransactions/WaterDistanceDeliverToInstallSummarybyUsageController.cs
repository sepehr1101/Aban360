using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/water-distnace-deliver-to-install-summary-by-Usage")]
    public class WaterDistanceDeliverToInstallSummaryByUsageController : BaseController
    {
        private readonly IWaterDistanceDeliverToInstallSummaryByUsageHandler _waterDistanceDeliverToInstallSummaryHandler;
        private readonly IReportGenerator _reportGenerator;
        public WaterDistanceDeliverToInstallSummaryByUsageController(
            IWaterDistanceDeliverToInstallSummaryByUsageHandler waterDistanceDeliverToInstallSummaryHandler,
            IReportGenerator reportGenerator)
        {
            _waterDistanceDeliverToInstallSummaryHandler = waterDistanceDeliverToInstallSummaryHandler;
            _waterDistanceDeliverToInstallSummaryHandler.NotNull(nameof(waterDistanceDeliverToInstallSummaryHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<SewageWaterDistanceHeaderOutputDto, SewageWaterDistanceSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WaterDistanceDeliverToInstallInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<SewageWaterDistanceHeaderOutputDto, SewageWaterDistanceSummaryDataOutputDto> result = await _waterDistanceDeliverToInstallSummaryHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, WaterDistanceDeliverToInstallInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _waterDistanceDeliverToInstallSummaryHandler.Handle, CurrentUser, ReportLiterals.WaterDistanceDeliverToInstallSummary, connectionId);
            return Ok(inputDto);
        }


        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(WaterDistanceDeliverToInstallInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 661;
            ReportOutput<SewageWaterDistanceHeaderOutputDto, SewageWaterDistanceSummaryDataOutputDto> result = await _waterDistanceDeliverToInstallSummaryHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
