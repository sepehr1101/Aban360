using Aban360.ClaimPool.Application.Features.Draft.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Queries
{
    [Route("v1/request-subscription")]
    public class RequestUserGetAllController : BaseController
    {
        private readonly IRequestUserGetAllHandler _requestUserGetAllHandler;
        public RequestUserGetAllController(IRequestUserGetAllHandler requestUserGetAllHandler)
        {
            _requestUserGetAllHandler = requestUserGetAllHandler;
            _requestUserGetAllHandler.NotNull(nameof(requestUserGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<RequestUserQueryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var RequestUsers = await _requestUserGetAllHandler.Handle(cancellationToken);
            return Ok(RequestUsers);
        }
    }
}
