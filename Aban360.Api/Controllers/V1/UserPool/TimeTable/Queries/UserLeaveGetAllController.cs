using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.TimeTable.Queries
{
    [Route("v4/user-leave")]
    public class UserLeaveGetAllController : BaseController
    {
        private readonly IUserLeaveGetAllHandler _userLeaveGetAllHandler;
        public UserLeaveGetAllController(IUserLeaveGetAllHandler userLeaveGetAllHandler)
        {
            _userLeaveGetAllHandler = userLeaveGetAllHandler;
            _userLeaveGetAllHandler.NotNull(nameof(userLeaveGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<UserLeaveGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var userLeaves = await _userLeaveGetAllHandler.Handle(cancellationToken);
            return Ok(userLeaves);
        }
    }
}
