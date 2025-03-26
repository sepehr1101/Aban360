using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.TimeTable.Queries
{
    [Route("v4/user-leave")]
    public class UserLeaveGetSingleController : BaseController
    {
        private readonly IUserLeaveGetSingleHandler _userLeaveGetSingleHandler;
        public UserLeaveGetSingleController(IUserLeaveGetSingleHandler UserLeaveGetSingleHandler)
        {
            _userLeaveGetSingleHandler = UserLeaveGetSingleHandler;
            _userLeaveGetSingleHandler.NotNull(nameof(UserLeaveGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UserLeaveGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var userLeaves = await _userLeaveGetSingleHandler.Handle(id, cancellationToken);
            return Ok(userLeaves);
        }
    }
}
