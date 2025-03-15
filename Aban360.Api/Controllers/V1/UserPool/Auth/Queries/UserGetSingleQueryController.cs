using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Queries
{
    [Route("v1/user")]
    public class UserGetSingleQueryController : BaseController
    {
        private readonly IUserGetSingleQueryHandler _userGetSingleQueryHandler;
        public UserGetSingleQueryController(IUserGetSingleQueryHandler userGetSingleQuery)
        {
            _userGetSingleQueryHandler = userGetSingleQuery;
            _userGetSingleQueryHandler.NotNull(nameof(userGetSingleQuery));
        }

        [AllowAnonymous]
        [HttpGet,HttpPost]
        [Route("info/{userId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UserDisplayDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInfo(Guid userId, CancellationToken cancellationToken)
        {
            UserDisplayDto userInfo= await _userGetSingleQueryHandler.Handle(userId, cancellationToken);
            return Ok(userInfo);
        }
    }
}
