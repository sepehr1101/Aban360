using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.UiElement.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.UiElement
{
    [Route("v1/account")]
    public class TopbarDisplayerController : BaseController
    {
        private readonly ITopbarQueryHandler _topbarQueryHandler;
        public TopbarDisplayerController(ITopbarQueryHandler topbarQueryHandler)
        {
            _topbarQueryHandler = topbarQueryHandler;
            _topbarQueryHandler.NotNull(nameof(topbarQueryHandler));
        }

        [HttpGet,HttpPost]
        [Route("my-topbar")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<Topbar>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyTopbar(CancellationToken cancellationToken)
        {
            Topbar topbar = await _topbarQueryHandler.Handle(CurrentUser.UserId, cancellationToken);
            return Ok(topbar);
        }

        [HttpGet, HttpPost]
        [Route("user-topbar/{userId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<Topbar>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyTopbar(Guid userId,CancellationToken cancellationToken)
        {
            Topbar topbar = await _topbarQueryHandler.Handle(userId, cancellationToken);
            return Ok(topbar);
        }
    }
}
