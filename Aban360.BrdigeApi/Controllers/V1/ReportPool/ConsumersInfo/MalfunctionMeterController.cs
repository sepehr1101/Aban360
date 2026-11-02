using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/bill")]
    public class MalfunctionMeterController : BaseController
    {
        private readonly IMalfunctionMeterHandler _malfunctionMeterHandler;
        public MalfunctionMeterController(IMalfunctionMeterHandler malfunctionMeterHandler)
        {
            _malfunctionMeterHandler = malfunctionMeterHandler;
            _malfunctionMeterHandler.NotNull(nameof(malfunctionMeterHandler));
        }

        [HttpPost]
        [Route("malfunction")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<MalfunctionMeterHeaderOutputDto, MalfunctionMeterDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMalfunctionBills(MalfunctionMeterInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<MalfunctionMeterHeaderOutputDto, MalfunctionMeterDataOutputDto> result = await _malfunctionMeterHandler.Handle(input, cancellationToken);
            return Ok(result);
        }
    }
}
