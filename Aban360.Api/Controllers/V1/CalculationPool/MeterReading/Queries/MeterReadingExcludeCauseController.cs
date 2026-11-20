using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.CalculationPool.Application.Features.MeterReading.Services;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Queries
{
    [Route("v1/meter-flow")]
    public class MeterReadingExcludeCauseController : BaseController
    {
        private readonly IMeterReadingExcludeCauseService _excludedCauseGetHandler;
        public MeterReadingExcludeCauseController(IMeterReadingExcludeCauseService excludedCauseGetHandler)
        {
            _excludedCauseGetHandler = excludedCauseGetHandler;
            _excludedCauseGetHandler.NotNull(nameof(excludedCauseGetHandler));
        }

        [HttpGet, HttpPost]
        [Route("exclude-cause/{isSelectable}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<MeterReadingExcludeCauseGetDto>>), StatusCodes.Status200OK)]
        public IActionResult GetExcludeCause(CancellationToken cancellationToken, bool isSelectable = false)
        {
            IEnumerable<MeterReadingExcludeCauseGetDto> result = _excludedCauseGetHandler.MeterReadingExcludeCauseGetAsync(isSelectable, cancellationToken);
            return Ok(result);
        }
    }
}
