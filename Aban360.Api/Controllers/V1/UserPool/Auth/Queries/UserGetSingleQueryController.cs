using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Microsoft.AspNetCore.Mvc;
using Aban360.UserPool.Application.Features.Auth.Services;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Queries
{
    [Route("v1/user")]
    public class UserGetSingleQueryController : BaseController
    {
        private readonly IUserManagementService _userManagementService;
        public UserGetSingleQueryController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
            _userManagementService.NotNull(nameof(userManagementService));
        }

        [HttpGet, HttpPost]
        [Route("info/{userId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UserDisplayDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInfo(Guid userId, CancellationToken cancellationToken)
        {
            UserDisplayDto userInfo = await _userManagementService.GetSingleAsync(userId, cancellationToken);
            return Ok(userInfo);
        }

        [HttpGet]
        [Route("is-admin")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> IsAdminGet(CancellationToken cancellationToken)
        {
            return Ok(CurrentUser.IsAdmin);
        }
    }
}
