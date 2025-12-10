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
    [Route("v1/consumption-average-management-summary")]
    public class ConsumptionAverageManagementSummaryController : BaseController
    {
        private readonly IConsumptionAverageManagementSummaryByUsageHandler _consumptionAverageManagerHandler;
        private readonly IReportGenerator _reportGenerator;
        public ConsumptionAverageManagementSummaryController(
            IConsumptionAverageManagementSummaryByUsageHandler consumptionAverageManagerHandler,
            IReportGenerator reportGenerator)
        {
            _consumptionAverageManagerHandler = consumptionAverageManagerHandler;
            _consumptionAverageManagerHandler.NotNull(nameof(consumptionAverageManagerHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<QuarterDto, KeyValueDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ConsumptionAverageManagementSummrayInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<QuarterDto, KeyValueDto> result = await _consumptionAverageManagerHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, ConsumptionAverageManagementSummrayInputDto   inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _consumptionAverageManagerHandler.Handle, CurrentUser, ReportLiterals.ConsumptionManagerDetail, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(ConsumptionAverageManagementSummrayInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 673;
            ReportOutput<QuarterDto, KeyValueDto> calculationDetails = await _consumptionAverageManagerHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(calculationDetails, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
