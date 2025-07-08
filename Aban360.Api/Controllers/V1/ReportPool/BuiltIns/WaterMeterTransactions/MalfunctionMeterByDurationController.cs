using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterByDurationTransactions
{
    [Route("v1/malfunction-meter-by-duration")]
    public class MalfunctionMeterByDurationController : BaseController
    {
        private readonly IMalfunctionMeterByDurationHandler _malfunctionMeterByDurationHandler;
        public MalfunctionMeterByDurationController(IMalfunctionMeterByDurationHandler malfunctionMeterByDurationHandler)
        {
            _malfunctionMeterByDurationHandler = malfunctionMeterByDurationHandler;
            _malfunctionMeterByDurationHandler.NotNull(nameof(malfunctionMeterByDurationHandler));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(MalfunctionMeterByDurationInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationDataOutputDto> result = await _malfunctionMeterByDurationHandler.Handle(input, cancellationToken);
            return Ok(result);
        }
    }
}
