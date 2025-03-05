using Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule.Queries
{
    [Route("v1/tariff")]
    public class TariffGetSingleController : BaseController
    {
        private readonly ITariffGetSingleHandler _tariffGetSingleHandler;
        public TariffGetSingleController(ITariffGetSingleHandler tariffGetSingleHandler)
        {
            _tariffGetSingleHandler = tariffGetSingleHandler;
            _tariffGetSingleHandler.NotNull(nameof(tariffGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            var tariffs = await _tariffGetSingleHandler.Handle(id, cancellationToken);
            return Ok(tariffs);
        }
    }
}
