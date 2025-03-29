using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/user-Workday")]
    public class UserWorkdayDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserWorkdayDeleteHandler _userWorkdayDeleteHandler;
        public UserWorkdayDeleteController(
            IUnitOfWork uow,
            IUserWorkdayDeleteHandler userWorkdayDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _userWorkdayDeleteHandler = userWorkdayDeleteHandler;
            _userWorkdayDeleteHandler.NotNull(nameof(userWorkdayDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UserWorkdayDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] UserWorkdayDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _userWorkdayDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
