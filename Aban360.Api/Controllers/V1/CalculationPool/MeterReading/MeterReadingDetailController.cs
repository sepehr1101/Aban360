using Aban360.Api.Filters;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Application.Features.MeterReading.Services;
using Aban360.Api.Controllers.V1.CalculationPool.MeterReading;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading
{
    [Route("v1/meter-reading-detail")]
    public class MeterReadingDetailController : BaseController
    {
        private readonly IMeterReadingDetailService _meterReadingDetailService;
        public MeterReadingDetailController(IMeterReadingDetailService meterReadingDetailService)
        {
            _meterReadingDetailService = meterReadingDetailService;
            _meterReadingDetailService.NotNull(nameof(meterReadingDetailService));
        }

        [HttpPost]
        [Route("exclude")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterReadingDetailExcludeInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Exclude([FromBody] MeterReadingDetailExcludeInputDto input, CancellationToken cancellationToken)
        {
            await _meterReadingDetailService.MeterReadingDetailExcludeAsync(input, CurrentUser, cancellationToken);
            return Ok(input);
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterReadingDetailCheckedDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(MeterReadingDetailUpdateDto input, CancellationToken cancellationToken)
        {
            MeterReadingDetailCheckedDto result = await _meterReadingDetailService.MeterReadingDetailUpdateAsync(input, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
