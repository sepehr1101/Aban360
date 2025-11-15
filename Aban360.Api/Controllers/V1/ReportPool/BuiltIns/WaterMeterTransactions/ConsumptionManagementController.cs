using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/consumption-management")]
    public class ConsumptionManagementController : BaseController
    {
        private readonly IConsumptionManagementHandler _consumptionManagerHandler;
        private readonly IReportGenerator _reportGenerator;
        public ConsumptionManagementController(
            IConsumptionManagementHandler consumptionManagerHandler,
            IReportGenerator reportGenerator)
        {
            _consumptionManagerHandler = consumptionManagerHandler;
            _consumptionManagerHandler.NotNull(nameof(consumptionManagerHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ConsumptionManagementHeaderOutputDto, ConsumptionManagementDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ConsumptionManagementInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<ConsumptionManagementHeaderOutputDto, ConsumptionManagementDataOutputDto> result = await _consumptionManagerHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, ConsumptionManagementInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _consumptionManagerHandler.Handle, CurrentUser, ReportLiterals.ConsumptionManagerDetail, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(ConsumptionManagementInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 670;
            ReportOutput<ConsumptionManagementHeaderOutputDto, ConsumptionManagementDataOutputDto> calculationDetails = await _consumptionManagerHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(calculationDetails, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
