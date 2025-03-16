using Aban360.ClaimPool.Application.Features.Draff.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Draff.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draff.Queries
{
    [Route("v1/request-user")]
    public class RequestUserGetSingleController : BaseController
    {
        private readonly IRequestUserGetSingleHandler _requestUserGetSingleHandler;
        public RequestUserGetSingleController(IRequestUserGetSingleHandler requestUserGetSingleHandler)
        {
            _requestUserGetSingleHandler = requestUserGetSingleHandler;
            _requestUserGetSingleHandler.NotNull(nameof(requestUserGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestUserQueryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var RequestUsers = await _requestUserGetSingleHandler.Handle(id, cancellationToken);
            return Ok(RequestUsers);
        }
    }
}
