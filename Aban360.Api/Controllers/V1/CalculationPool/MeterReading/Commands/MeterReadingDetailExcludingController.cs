using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Commands
{
    [Route("v1/meter-reading-detail")]
    public class MeterReadingDetailExcludingController : BaseController
    {
        private readonly IMeterReadingDetailExcludeHandler _meterReadingDetailExcludeHandler;
        public MeterReadingDetailExcludingController(
            IMeterReadingDetailExcludeHandler meterReadingDetailExcludedHandler)
        {
            _meterReadingDetailExcludeHandler = meterReadingDetailExcludedHandler;
            _meterReadingDetailExcludeHandler.NotNull(nameof(meterReadingDetailExcludedHandler));
        }

        [HttpPost]
        [Route("exclude")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterReadingDetailExcludeInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Exclude([FromBody] MeterReadingDetailExcludeInputDto input, CancellationToken cancellationToken)
        {
            await _meterReadingDetailExcludeHandler.Handle(input, CurrentUser, cancellationToken);
            return Ok(input);
        }
    }
}
