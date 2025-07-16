using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/ruined-meter-income-detail")]
    public class RuinedMeterIncomeDetailController : BaseController
    {
        private readonly IRuinedMeterIncomeDetailHandler _reuinedMeterIncomeDetailHandler;
        public RuinedMeterIncomeDetailController(IRuinedMeterIncomeDetailHandler reuinedMeterIncomeDetailHandler)
        {
            _reuinedMeterIncomeDetailHandler = reuinedMeterIncomeDetailHandler;
            _reuinedMeterIncomeDetailHandler.NotNull(nameof(reuinedMeterIncomeDetailHandler));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<RuinedMeterIncomeHeaderOutputDto, RuinedMeterIncomeDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(RuinedMeterIncomeInputDto input, CancellationToken cancellationToken)
        {
            var result = await _reuinedMeterIncomeDetailHandler.Handle(input, cancellationToken);
            return Ok(result);
        }
    }
}
