using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Commands
{
    [Obsolete]
    [Route("v1/meter-flow")]
    public class MeterFlowInitialCalculationController : BaseController
    {
        private readonly IInitialCalculationHandler _initialCalculationHandler;
        public MeterFlowInitialCalculationController(
             IInitialCalculationHandler initialCalculationHandler)
        {
            _initialCalculationHandler = initialCalculationHandler;
            _initialCalculationHandler.NotNull(nameof(initialCalculationHandler));
        }

        [HttpPost]
        [Route("initial-calculate/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<MeterReadingDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Calculate(int id, CancellationToken cancellationToken)
        {            
            IEnumerable<MeterReadingDetailDataOutputDto> result= await _initialCalculationHandler.Handle(id, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
