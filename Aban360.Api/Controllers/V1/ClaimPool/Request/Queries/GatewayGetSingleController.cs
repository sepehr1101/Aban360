using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Queries
{
    [Route("v1/gateway")]
    public class GatewayGetSingleController : BaseController
    {
        private readonly IGatewayGetSingleHandler _gatewayGetSingleHandler;
        public GatewayGetSingleController(IGatewayGetSingleHandler getewayGetSingleHandler)
        {
            _gatewayGetSingleHandler = getewayGetSingleHandler;
            _gatewayGetSingleHandler.NotNull(nameof(getewayGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<GatewayGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var gateways = await _gatewayGetSingleHandler.Handle(id, cancellationToken);
            return Ok(gateways);
        }
    }
}
