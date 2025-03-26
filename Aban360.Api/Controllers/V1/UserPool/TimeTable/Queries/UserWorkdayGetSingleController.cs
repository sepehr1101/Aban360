using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.TimeTable.Queries
{
    [Route("v4/user-Workday")]
    public class UserWorkdayGetSingleController : BaseController
    {
        private readonly IUserWorkdayGetSingleHandler _userWorkdayGetSingleHandler;
        public UserWorkdayGetSingleController(IUserWorkdayGetSingleHandler UserWorkdayGetSingleHandler)
        {
            _userWorkdayGetSingleHandler = UserWorkdayGetSingleHandler;
            _userWorkdayGetSingleHandler.NotNull(nameof(UserWorkdayGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UserWorkdayGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var userWorkdays = await _userWorkdayGetSingleHandler.Handle(id, cancellationToken);
            return Ok(userWorkdays);
        }
    }
}
