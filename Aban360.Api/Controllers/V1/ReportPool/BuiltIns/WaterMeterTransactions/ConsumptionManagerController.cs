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
    [Route("v1/consumption-manager")]
    public class ConsumptionManagerController : BaseController
    {
        private readonly IConsumptionManagerHandler _consumptionManagerHandler;
        private readonly IReportGenerator _reportGenerator;
        public ConsumptionManagerController(
            IConsumptionManagerHandler consumptionManagerHandler,
            IReportGenerator reportGenerator)
        {
            _consumptionManagerHandler = consumptionManagerHandler;
            _consumptionManagerHandler.NotNull(nameof(consumptionManagerHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ConsumptionManagerHeaderOutputDto, ConsumptionManagerDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ConsumptionManagerInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<ConsumptionManagerHeaderOutputDto, ConsumptionManagerDataOutputDto> result = await _consumptionManagerHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, ConsumptionManagerInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _consumptionManagerHandler.Handle, CurrentUser, ReportLiterals.ConsumptionManagerDetail, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(ConsumptionManagerInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 670;
            ReportOutput<ConsumptionManagerHeaderOutputDto, ConsumptionManagerDataOutputDto> calculationDetails = await _consumptionManagerHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(calculationDetails, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
