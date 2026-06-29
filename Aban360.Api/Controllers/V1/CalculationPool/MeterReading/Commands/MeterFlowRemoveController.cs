using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Delete.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Commands
{
    [Route("v1/meter-flow")]
    public class MeterFlowRemoveController : BaseController
    {
        private readonly IMeterReadingFileRemoveHandler _meterReadingFileRemoveHandler;
        public MeterFlowRemoveController(IMeterReadingFileRemoveHandler meterReadingFileRemoveHandler)
        {
            _meterReadingFileRemoveHandler = meterReadingFileRemoveHandler;
            _meterReadingFileRemoveHandler.NotNull(nameof(meterReadingFileRemoveHandler));
        }

        [HttpPost]
        [Route("remove/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveFile(int id, CancellationToken cancellationToken)
        {
            await _meterReadingFileRemoveHandler.Handle(id, CurrentUser, cancellationToken);
            return Ok(id);
        }
    }
}
