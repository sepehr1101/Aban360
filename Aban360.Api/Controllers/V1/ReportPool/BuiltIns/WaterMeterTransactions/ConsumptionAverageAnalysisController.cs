using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/consumption-average-analysis")]
    public class ConsumptionAverageAnalysisController : BaseController
    {
        private readonly IConsumptionAverageAnalysisHandler _consumptionAverageAnalysisHandler;
        private readonly IReportGenerator _reportGenerator;
        public ConsumptionAverageAnalysisController(
            IConsumptionAverageAnalysisHandler consumptionAverageAnalysisHandler,
            IReportGenerator reportGenerator)
        {
            _consumptionAverageAnalysisHandler = consumptionAverageAnalysisHandler;
            _consumptionAverageAnalysisHandler.NotNull(nameof(consumptionAverageAnalysisHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(reportGenerator));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ConsumptionAverageAnalysisHeaderOutputDto, ConsumptionAverageAnalysisDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ConsumptionAverageAnalysisInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<ConsumptionAverageAnalysisHeaderOutputDto, ConsumptionAverageAnalysisDataOutputDto> result = await _consumptionAverageAnalysisHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(ConsumptionAverageAnalysisInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 679;
            ReportOutput<ConsumptionAverageAnalysisHeaderOutputDto, ConsumptionAverageAnalysisDataOutputDto> result = await _consumptionAverageAnalysisHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
