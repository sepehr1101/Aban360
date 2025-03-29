using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v4/user-leave")]
    public class UserLeaveDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserLeaveDeleteHandler _userLeaveDeleteHandler;
        public UserLeaveDeleteController(
            IUnitOfWork uow,
            IUserLeaveDeleteHandler userLeaveDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _userLeaveDeleteHandler = userLeaveDeleteHandler;
            _userLeaveDeleteHandler.NotNull(nameof(userLeaveDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UserLeaveDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] UserLeaveDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _userLeaveDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
