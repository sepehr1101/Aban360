using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.CalculationPool.Application.Features.MeterReading.Services;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Commands
{
    [Obsolete]
    [Route("v1/meter-flow")]
    public class MeterFlowInitialCalculationController : BaseController
    {
        private readonly IMeterFlowService _meterFlowService;
        public MeterFlowInitialCalculationController(IMeterFlowService meterFlowService)
        {
            _meterFlowService = meterFlowService;
            _meterFlowService.NotNull(nameof(meterFlowService));
        }

        [HttpPost]
        [Route("initial-calculate/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<MeterReadingDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Calculate(int id, CancellationToken cancellationToken)
        {
            IEnumerable<MeterReadingDetailDataOutputDto> result = await _meterFlowService.InitialCalculationAsync(id, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
