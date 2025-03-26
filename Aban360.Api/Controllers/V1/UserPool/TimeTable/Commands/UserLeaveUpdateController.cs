using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.TimeTable.Commands
{
    [Route("v4/user-leave")]
    public class UserLeaveUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserLeaveUpdateHandler _userLeaveUpdateHandler;
        public UserLeaveUpdateController(
            IUnitOfWork uow,
            IUserLeaveUpdateHandler userLeaveUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _userLeaveUpdateHandler = userLeaveUpdateHandler;
            _userLeaveUpdateHandler.NotNull(nameof(userLeaveUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] UserLeaveUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _userLeaveUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
