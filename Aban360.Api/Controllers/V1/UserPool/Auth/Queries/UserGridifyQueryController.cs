using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Gridify;
using Microsoft.AspNetCore.Mvc;
using Aban360.UserPool.Application.Features.Auth.Services;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Queries
{
    [Route("v1/user")]
    public class UserGridifyQueryController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;
        public UserGridifyQueryController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
            _userManagementService.NotNull(nameof(userManagementService));
        }

        [HttpGet, HttpPost]
        [Route("query")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<UserQueryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsersByQuery([FromQuery] GridifyQuery query, CancellationToken cancellationToken)
        {
            ICollection<UserQueryDto> userQueryDtos = await _userManagementService.GridifyAsync(query, cancellationToken);
            ;
            return Ok(userQueryDtos);
        }
    }
}
