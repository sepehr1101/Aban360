using Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule.Queries
{
    [Route("v1/tariff-by-detail")]
    public class TariffByDetailGetSingleController : BaseController
    {
        private readonly ITariffByDetailGetSingleHandler _tariffByDetailGetSingleHandler;
        public TariffByDetailGetSingleController(ITariffByDetailGetSingleHandler tariffByDetailGetSingleHandler)
        {
            _tariffByDetailGetSingleHandler = tariffByDetailGetSingleHandler;
            _tariffByDetailGetSingleHandler.NotNull(nameof(tariffByDetailGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffByDetailGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            var tariffByDetails = await _tariffByDetailGetSingleHandler.Handle(id, cancellationToken);
            return Ok(tariffByDetails);
        }
    }
}
