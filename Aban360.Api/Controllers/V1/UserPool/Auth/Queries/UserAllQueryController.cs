using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Queries
{    
    [Route("v1/user")]
    public class UserAllQueryController : ControllerBase
    {
        private readonly IUserAllQueryHandler _userAllQueryHandler;
        public UserAllQueryController(IUserAllQueryHandler userAllQueryHandler)
        {
            _userAllQueryHandler = userAllQueryHandler;
            _userAllQueryHandler.NotNull(nameof(userAllQueryHandler));
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<UserQueryDto>>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var usersDto= await _userAllQueryHandler.Handle(cancellationToken);
            return Ok(usersDto);
        }
    }
}
