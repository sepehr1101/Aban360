using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading
{
    public class MeterReadingDetailController : BaseController
    {
        private readonly IMeterReadingDetailExcludeHandler _c0_meterReadingDetailExcludeHandler;
        private readonly IMeterReadingDetailUpdateHandler _c1_meterReadingDetailUpdateHandler;
        public MeterReadingDetailController(IMeterReadingDetailExcludeHandler c0_meterReadingDetailExcludedHandler, IMeterReadingDetailUpdateHandler c1_meterReadingDetailUpdateHandler)
        {
            _c0_meterReadingDetailExcludeHandler = c0_meterReadingDetailExcludedHandler;
            _c0_meterReadingDetailExcludeHandler.NotNull(nameof(c0_meterReadingDetailExcludedHandler));
            _c1_meterReadingDetailUpdateHandler = c1_meterReadingDetailUpdateHandler;
            _c1_meterReadingDetailUpdateHandler.NotNull(nameof(c1_meterReadingDetailUpdateHandler));
        }

        [HttpPost]
        [Route("~/v1/meter-reading-detail/exclude")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterReadingDetailExcludeInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Exclude([FromBody] MeterReadingDetailExcludeInputDto input, CancellationToken cancellationToken)
        {
            await _c0_meterReadingDetailExcludeHandler.Handle(input, CurrentUser, cancellationToken);
            return Ok(input);
        }

        [HttpPost]
        [Route("~/v1/meter-reading-detail/update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterReadingDetailCheckedDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(MeterReadingDetailUpdateDto input, CancellationToken cancellationToken)
        {
            MeterReadingDetailCheckedDto result = await _c1_meterReadingDetailUpdateHandler.Handle(input, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
