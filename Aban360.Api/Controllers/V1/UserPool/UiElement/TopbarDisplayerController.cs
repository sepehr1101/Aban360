using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Microsoft.AspNetCore.Mvc;
using Aban360.UserPool.Application.Features.AccessTree.Services;

namespace Aban360.Api.Controllers.V1.UserPool.UiElement
{
    [Route("v1/account")]
    public class TopbarDisplayerController : BaseController
    {
        private readonly IUserNavigationService _userNavigationService;
        public TopbarDisplayerController(IUserNavigationService userNavigationService)
        {
            _userNavigationService = userNavigationService;
            _userNavigationService.NotNull(nameof(userNavigationService));
        }

        [HttpGet, HttpPost]
        [Route("my-topbar")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<Topbar>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyTopbar(CancellationToken cancellationToken)
        {
            Topbar topbar = await _userNavigationService.GetTopbarAsync(CurrentUser.UserId, cancellationToken);
            return Ok(topbar);
        }

        [HttpGet, HttpPost]
        [Route("user-topbar/{userId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<Topbar>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserTopbar(Guid userId, CancellationToken cancellationToken)
        {
            Topbar topbar = await _userNavigationService.GetTopbarAsync(userId, cancellationToken);
            return Ok(topbar);
        }
    }
}
