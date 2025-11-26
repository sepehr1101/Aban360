using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Commands
{
    [Route("v1/meter-reading-detail")]
    public class MeterReadingDetailUpdateController : BaseController
    {
        private readonly IMeterReadingDetailUpdateHandler _meterReadingDetailUpdateHandler;
        public MeterReadingDetailUpdateController(
            IMeterReadingDetailUpdateHandler meterReadingDetailUpdateHandler)
        {
            _meterReadingDetailUpdateHandler = meterReadingDetailUpdateHandler;
            _meterReadingDetailUpdateHandler.NotNull(nameof(meterReadingDetailUpdateHandler));
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<MeterReadingDetailCheckedDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(MeterReadingDetailUpdateDto input, CancellationToken cancellationToken)
        {
            await _meterReadingDetailUpdateHandler.Handle(input, CurrentUser, cancellationToken);
            return Ok(input);
        }
    }
}
