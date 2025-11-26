using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Excluded.Implementations;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Commands
{
    [Route("v1/meter-reading-detail")]
    public class MeterReadingDetailExcludedController : BaseController
    {
        private readonly IMeterReadingDetailExcludedHandler _meterReadingDetailExcludedHandler;
        public MeterReadingDetailExcludedController(
            IMeterReadingDetailExcludedHandler meterReadingDetailExcludedHandler)
        {
            _meterReadingDetailExcludedHandler = meterReadingDetailExcludedHandler;
            _meterReadingDetailExcludedHandler.NotNull(nameof(meterReadingDetailExcludedHandler));
        }

        [HttpPost]
        [Route("excluded/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Excluded(int id, CancellationToken cancellationToken)
        {
            await _meterReadingDetailExcludedHandler.Handle(id, CurrentUser, cancellationToken);
            return Ok(id);
        }
    }
}
