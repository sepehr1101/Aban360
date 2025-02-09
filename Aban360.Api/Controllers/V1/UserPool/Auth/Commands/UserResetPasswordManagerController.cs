using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Base;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Commands
{
    [Route("v1/user")]
    public class UserResetPasswordManagerController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserResetPasswordCommandHandler _userResetPasswordCommandHandler;
        public UserResetPasswordManagerController(
            IUnitOfWork uow,
            IUserResetPasswordCommandHandler userResetPasswordCommandHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _userResetPasswordCommandHandler = userResetPasswordCommandHandler;
            _userResetPasswordCommandHandler.NotNull(nameof(userResetPasswordCommandHandler));
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] UserIdDto userId, CancellationToken cancellationToken)
        {
            await _userResetPasswordCommandHandler.Handle(userId, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok("وضعیت کاربر به -فعال- ویرایش شد");
        }
    }
}
