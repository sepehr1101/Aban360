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
        public UserSearchController(IUserSearchHandler userSearch)
        {
            _userSearch = userSearch;
            _userSearch.NotNull(nameof(userSearch));
        }

        [HttpGet, HttpPost]
        [Route("search")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<UserQueryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetParamsOfCreate(SearchUserDto inputDto,CancellationToken cancellationToken)
        {
            IEnumerable<UserQueryDto> userParamsOfCreateDto = await _userSearch.Handle(inputDto,cancellationToken);
            return Ok(userParamsOfCreateDto);
        }
    }
}
