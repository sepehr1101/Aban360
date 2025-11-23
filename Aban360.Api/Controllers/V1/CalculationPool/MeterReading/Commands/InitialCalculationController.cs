using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Commands
{
    [Route("v1/initial-calculation")]
    public class InitialCalculationController : BaseController
    {
        private readonly IInitialCalculationHandler _initialCalculationHandler;
        public InitialCalculationController(IInitialCalculationHandler initialCalculationHandler)
        {
            _initialCalculationHandler = initialCalculationHandler;
            _initialCalculationHandler.NotNull(nameof(initialCalculationHandler));
        }

        [HttpPost]
        [Route("calculation")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<MeterReadingDetailGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Calculation(int id, CancellationToken cancellationToken)
        {
            IEnumerable<MeterReadingDetailGetDto> result= await _initialCalculationHandler.Handle(id, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
