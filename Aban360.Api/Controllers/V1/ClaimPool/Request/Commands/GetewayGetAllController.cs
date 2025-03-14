using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/geteway")]
    public class GetewayGetAllController : BaseController
    {
        private readonly IGetewayGetAllHandler _getewayGetAllHandler;
        public GetewayGetAllController(IGetewayGetAllHandler getewayGetAllHandler)
        {
            _getewayGetAllHandler = getewayGetAllHandler;
            _getewayGetAllHandler.NotNull(nameof(getewayGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<GetewayGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var geteways = await _getewayGetAllHandler.Handle(cancellationToken);
            return Ok(geteways);
        }
    }
}
