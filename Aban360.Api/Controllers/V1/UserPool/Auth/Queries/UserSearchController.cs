using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Microsoft.AspNetCore.Mvc;
using Aban360.UserPool.Application.Features.Auth.Services;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Queries
{
    [Route("v1/user")]
    public class UserSearchController : BaseController
    {
        private readonly IUserSearchService _userSearchService;
        public UserSearchController(IUserSearchService userSearchService)
        {
            _userSearchService = userSearchService;
            _userSearchService.NotNull(nameof(userSearchService));
        }

        [HttpPost]
        [Route("search")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<UserQueryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search(SearchUserDto inputDto, CancellationToken cancellationToken)
        {
            IEnumerable<UserQueryDto> userParamsOfCreateDto = await _userSearchService.SearchAsync(inputDto, cancellationToken);
            return Ok(userParamsOfCreateDto);
        }

        [HttpGet, HttpPost]
        [Route("search-by-role/{roleId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<UserQueryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchByRoleId(int roleId, CancellationToken cancellationToken)
        {
            IEnumerable<UserQueryDto> users = await _userSearchService.SearchByRoleAsync(roleId, cancellationToken);
            return Ok(users);
        }

        [HttpGet, HttpPost]
        [Route("search-by-role-zone/{roleId}/{zoneId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<UserQueryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchByRoleIdAndZoneId(int roleId, int zoneId, CancellationToken cancellationToken)
        {
            IEnumerable<UserQueryDto> users = await _userSearchService.SearchByRoleAndZoneAsync(roleId, zoneId, cancellationToken);
            return Ok(users);
        }
    }
}
