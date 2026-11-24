using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Reading.Services;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.MeterPool.Reading
{
    [Route("v1/meter-reading")]
    public class MeterReadingSetPreStepController : BaseController
    {
        private readonly ISetPreviousStepService _setPreviousStepService;
        public MeterReadingSetPreStepController(ISetPreviousStepService setPreviousStepService)
        {
            _setPreviousStepService = setPreviousStepService;
            _setPreviousStepService.NotNull(nameof(setPreviousStepService));
        }

        [Route("re-generate-list/{trackNumber}")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetReGenerateList(int trackNumber, CancellationToken cancellationToken)
        {
            await _setPreviousStepService.SetReGenerateListAsync(trackNumber, CurrentUser, cancellationToken);
            return Ok(trackNumber);
        }
        
        [Route("re-offload/{trackNumber}")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetReOffLoadList(int trackNumber, CancellationToken cancellationToken)
        {
            await _setPreviousStepService.SetReStartReadingAsync(trackNumber, CurrentUser, cancellationToken);
            return Ok(trackNumber);
        }
        
        [Route("re-finish/{trackNumber}")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetReFinish(int trackNumber, CancellationToken cancellationToken)
        {
            await _setPreviousStepService.SetReFinishByReaderAsync(trackNumber, CurrentUser, cancellationToken);
            return Ok(trackNumber);
        }
    }
}
