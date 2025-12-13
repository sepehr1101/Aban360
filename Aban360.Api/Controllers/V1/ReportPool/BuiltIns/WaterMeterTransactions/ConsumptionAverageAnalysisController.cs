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
        public ConsumptionAverageAnalysisController(
            IConsumptionAverageAnalysisHandler consumptionAverageAnalysisHandler)
        {
            _consumptionAverageAnalysisHandler = consumptionAverageAnalysisHandler;
            _consumptionAverageAnalysisHandler.NotNull(nameof(consumptionAverageAnalysisHandler));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ConsumptionAverageAnalysisHeaderOutputDto, ConsumptionAverageAnalysisDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ConsumptionAverageAnalysisInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<ConsumptionAverageAnalysisHeaderOutputDto, ConsumptionAverageAnalysisDataOutputDto> result = await _consumptionAverageAnalysisHandler.Handle(input, cancellationToken);
            return Ok(result);
        }
    }
}
