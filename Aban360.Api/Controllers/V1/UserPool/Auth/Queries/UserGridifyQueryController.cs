using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Queries
{
    [Route("v1/user")]
    public class UserGridifyQueryController : ControllerBase
    {
        private readonly IUserGridifyQueryHandler _userGridifyQueryHandler;

        public UserGridifyQueryController(IUserGridifyQueryHandler userGridifyQueryHandler)
        {
            _userGridifyQueryHandler = userGridifyQueryHandler;
            _userGridifyQueryHandler.NotNull(nameof(userGridifyQueryHandler));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("query")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<UserQueryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsersByQuery([FromQuery] GridifyQuery query, CancellationToken cancellationToken)
        {
            var userQueryDtos = await _userGridifyQueryHandler.Handle(query, cancellationToken); ;
            return Ok(userQueryDtos);
        }
    }
}
