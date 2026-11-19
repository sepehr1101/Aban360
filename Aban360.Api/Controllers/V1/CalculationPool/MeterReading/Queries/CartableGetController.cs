using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.CollectBills.Inputs;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.CalculationPool.Application.Features.MeterReading.Services;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Queries
{
    [Route("v1/cartable")]
    public class CartableGetController : BaseController
    {
        private readonly IMeterReadingCartableService _meterReadingCartableService;
        public CartableGetController(IMeterReadingCartableService meterReadingCartableService)
        {
            _meterReadingCartableService = meterReadingCartableService;
            _meterReadingCartableService.NotNull(nameof(meterReadingCartableService));
        }

        [HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<MeterFlowCartableGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            IEnumerable<MeterFlowCartableGetDto> result = await _meterReadingCartableService.CartableAsync(CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("get-completed")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<MeterFlowCartableGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompleted([FromBody] MeterFlowByZoneInputDto inputDto, CancellationToken cancellationToken)
        {
            IEnumerable<MeterFlowCartableGetDto> result = await _meterReadingCartableService.CartableByZoneIdGetAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
