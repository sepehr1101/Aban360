using Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule.Queries
{
    [Route("v1/tariff-calculation-mode")]
    public class TariffCalculationModeGetSingleController : BaseController
    {
        private readonly ITariffCalculationModeGetSingleHandler _tariffCalculationModeGetSingleHandler;
        public TariffCalculationModeGetSingleController(ITariffCalculationModeGetSingleHandler tariffCalculationModeGetSingleHandler)
        {
            _tariffCalculationModeGetSingleHandler = tariffCalculationModeGetSingleHandler;
            _tariffCalculationModeGetSingleHandler.NotNull(nameof(tariffCalculationModeGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffCalculationModeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var tariffCalculationModes = await _tariffCalculationModeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(tariffCalculationModes);
        }
    }
}
