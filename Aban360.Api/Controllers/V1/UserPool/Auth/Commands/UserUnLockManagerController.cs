using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Base;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Commands
{
    [Route("v1/user")]
    public class UserUnLockManagerController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserUnLockCommandHandler _userUnLockCommandHandler;
        public UserUnLockManagerController(
            IUnitOfWork uow,
            IUserUnLockCommandHandler userUnLockCommandHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _userUnLockCommandHandler = userUnLockCommandHandler;
            _userUnLockCommandHandler.NotNull(nameof(userUnLockCommandHandler));
        }

        [HttpPost]
        [Route("unlock")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UnLock([FromBody] UserIdDto userId, CancellationToken cancellationToken)
        {
            await _userUnLockCommandHandler.Handle(userId, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok("وضعیت کاربر به -غیرفعال- ویرایش شد");
        }
    }
}
