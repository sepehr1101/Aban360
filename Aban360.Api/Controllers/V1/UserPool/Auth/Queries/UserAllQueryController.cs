using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Microsoft.AspNetCore.Mvc;
using Aban360.UserPool.Application.Features.Auth.Services;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Queries
{
    [Route("v1/user")]
    public class UserAllQueryController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;
        public UserAllQueryController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
            _userManagementService.NotNull(nameof(userManagementService));
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<UserQueryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            ICollection<UserQueryDto> usersDto = await _userManagementService.AllAsync(cancellationToken);
            return Ok(usersDto);
        }

        [HttpGet, HttpPost]
        [Route("all-brief")]
        [ProducesResponseType(typeof(ICollection<UserBriefQueryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBrief(CancellationToken cancellationToken)
        {
            ICollection<UserBriefQueryDto> usersDto = await _userManagementService.BriefAsync(cancellationToken);
            return Ok(usersDto);
        }
    }
}
