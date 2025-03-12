using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/geteway")]
    public class GetewayGetSingleController : BaseController
    {
        private readonly IGetewayGetSingleHandler _getewayGetSingleHandler;
        public GetewayGetSingleController(IGetewayGetSingleHandler getewayGetSingleHandler)
        {
            _getewayGetSingleHandler = getewayGetSingleHandler;
            _getewayGetSingleHandler.NotNull(nameof(getewayGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<GetewayGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var geteways = await _getewayGetSingleHandler.Handle(id, cancellationToken);
            return Ok(geteways);
        }
    }
}
