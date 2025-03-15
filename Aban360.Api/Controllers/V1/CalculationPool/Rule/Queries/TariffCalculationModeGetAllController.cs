using Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule.Queries
{
    [Route("v1/tariff-calculation-mode")]
    public class TariffCalculationModeGetAllController : BaseController
    {
        private readonly ITariffCalculationModeGetAllHandler _tariffCalculationModeGetAllHandler;
        public TariffCalculationModeGetAllController(ITariffCalculationModeGetAllHandler tariffCalculationModeGetAllHandler)
        {
            _tariffCalculationModeGetAllHandler = tariffCalculationModeGetAllHandler;
            _tariffCalculationModeGetAllHandler.NotNull(nameof(tariffCalculationModeGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<TariffCalculationModeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<TariffCalculationModeGetDto> tariffCalculationModes = await _tariffCalculationModeGetAllHandler.Handle(cancellationToken);
            return Ok(tariffCalculationModes);
        }
    }
}
