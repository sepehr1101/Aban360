using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
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
