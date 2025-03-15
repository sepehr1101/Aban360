using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Base;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Commands
{
    [Route("v1/user")]
    public class UserLockManagerController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserLockCommandHandler _userLockCommandHandler;
        public UserLockManagerController(
            IUnitOfWork uow,
            IUserLockCommandHandler userLockCommandHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _userLockCommandHandler = userLockCommandHandler;
            _userLockCommandHandler.NotNull(nameof(userLockCommandHandler));
        }

        [HttpPost]
        [Route("lock")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Lock([FromBody] UserIdDto userId, CancellationToken cancellationToken)
        {
            await _userLockCommandHandler.Handle(userId, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok("وضعیت کاربر به -فعال- ویرایش شد");
        }
    }
}
