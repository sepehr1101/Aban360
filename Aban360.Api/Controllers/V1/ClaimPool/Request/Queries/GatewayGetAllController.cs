using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Queries
{
    [Route("v1/gateway")]
    public class GatewayGetAllController : BaseController
    {
        private readonly IGatewayGetAllHandler _gatewayGetAllHandler;
        public GatewayGetAllController(IGatewayGetAllHandler getewayGetAllHandler)
        {
            _gatewayGetAllHandler = getewayGetAllHandler;
            _gatewayGetAllHandler.NotNull(nameof(getewayGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<GatewayGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var gateways = await _gatewayGetAllHandler.Handle(cancellationToken);
            return Ok(gateways);
        }
    }
}
