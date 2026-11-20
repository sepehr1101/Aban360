using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.CalculationPool.Application.Features.MeterReading.Services;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Commands
{
    [Route("v1/meter-flow")]
    public class MeterFlowRemoveController : BaseController
    {
        private readonly IMeterReadingFileRemovalService _meterReadingFileRemoveHandler;
        public MeterFlowRemoveController(IMeterReadingFileRemovalService meterReadingFileRemoveHandler)
        {
            _meterReadingFileRemoveHandler = meterReadingFileRemoveHandler;
            _meterReadingFileRemoveHandler.NotNull(nameof(meterReadingFileRemoveHandler));
        }

        [HttpPost, HttpGet]
        [Route("remove/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveFile(int id, CancellationToken cancellationToken)
        {
            await _meterReadingFileRemoveHandler.MeterReadingFileRemoveAsync(id, CurrentUser, cancellationToken);
            return Ok(id);
        }
    }
}
