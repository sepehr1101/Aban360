using Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule.Queries
{
    [Route("v1/tariff-constant")]
    public class TariffConstantGetSingleController : BaseController
    {
        private readonly ITariffConstantGetSingleHandler _tariffConstantGetSingleHandler;
        public TariffConstantGetSingleController(ITariffConstantGetSingleHandler tariffConstantGetSingleHandler)
        {
            _tariffConstantGetSingleHandler = tariffConstantGetSingleHandler;
            _tariffConstantGetSingleHandler.NotNull(nameof(tariffConstantGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffConstantGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var tariffConstants = await _tariffConstantGetSingleHandler.Handle(id, cancellationToken);
            return Ok(tariffConstants);
        }
    }
}
