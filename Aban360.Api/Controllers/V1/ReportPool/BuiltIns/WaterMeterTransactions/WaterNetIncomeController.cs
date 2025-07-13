using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/water-net-income")]
    public class WaterNetIncomeController : BaseController
    {
        private readonly IWaterNetIncomeHandler _waterNetIncomeHandler;
        public WaterNetIncomeController(IWaterNetIncomeHandler waterNetIncomeHandler)
        {
            _waterNetIncomeHandler = waterNetIncomeHandler;
            _waterNetIncomeHandler.NotNull(nameof(waterNetIncomeHandler));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterNetIncomeHeaderOutputDto, WaterNetIncomeDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WaterNetIncomeInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<WaterNetIncomeHeaderOutputDto, WaterNetIncomeDataOutputDto> WaterNetIncome = await _waterNetIncomeHandler.Handle(input, cancellationToken);
            return Ok(WaterNetIncome);
        }
    }
}
