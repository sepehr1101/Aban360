using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Queries
{
    [Route("v1/user")]
    public class UserSearchController : BaseController
    {
        private readonly IUserSearchHandler _userSearch;
        private readonly IUserSearchByRoleIdHandler _userSearchByRoleIdHandler;
        public UserSearchController(
            IUserSearchHandler userSearch, 
            IUserSearchByRoleIdHandler userSearchByRoleIdHandler)
        {
            _userSearch = userSearch;
            _userSearch.NotNull(nameof(userSearch));
            
            _userSearchByRoleIdHandler = userSearchByRoleIdHandler;
            _userSearchByRoleIdHandler.NotNull(nameof(userSearchByRoleIdHandler));  
        }

        [HttpGet, HttpPost]
        [Route("search")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<UserQueryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetParamsOfCreate(SearchUserDto inputDto,CancellationToken cancellationToken)
        {
            IEnumerable<UserQueryDto> userParamsOfCreateDto = await _userSearch.Handle(inputDto,cancellationToken);
            return Ok(userParamsOfCreateDto);
        }

        [HttpGet, HttpPost]
        [Route("search-by-role/{roleId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<UserQueryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetParamsOfCreate(int roleId,CancellationToken cancellationToken)
        {
            IEnumerable<UserQueryDto> users = await _userSearchByRoleIdHandler.Handle(roleId, cancellationToken);
            return Ok(users);
        }
    }
}
