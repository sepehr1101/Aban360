using Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Implementations;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule.Queries
{
    [Route("v1/tanker-tariff")]
    public class TankerTariffController : BaseController
    {
        private readonly ITankerTariffGetAllHandler _tankerTariffGetAllHandler;
        public TankerTariffController(ITankerTariffGetAllHandler tankerTariffGetAllHandler)
        {
            _tankerTariffGetAllHandler = tankerTariffGetAllHandler;
            _tankerTariffGetAllHandler.NotNull(nameof(tankerTariffGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<TankerTariffGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll( CancellationToken cancellationToken)
        {
            IEnumerable<TankerTariffGetDto> tariffs = await _tankerTariffGetAllHandler.Handle( cancellationToken);
            return Ok(tariffs);
        }
    }
}
