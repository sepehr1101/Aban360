using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
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
