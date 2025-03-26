using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.TimeTable.Queries
{
    [Route("v4/user-Workday")]
    public class UserWorkdayGetAllController : BaseController
    {
        private readonly IUserWorkdayGetAllHandler _userWorkdayGetAllHandler;
        public UserWorkdayGetAllController(IUserWorkdayGetAllHandler userWorkdayGetAllHandler)
        {
            _userWorkdayGetAllHandler = userWorkdayGetAllHandler;
            _userWorkdayGetAllHandler.NotNull(nameof(userWorkdayGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<UserWorkdayGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var userWorkdays = await _userWorkdayGetAllHandler.Handle(cancellationToken);
            return Ok(userWorkdays);
        }
    }
}
