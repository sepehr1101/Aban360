using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Base;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Aban360.UserPool.Application.Features.Auth.Services;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Commands
{
    [Route("v1/user")]
    public class UserResetPasswordManagerController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserPasswordService _userPasswordService;
        public UserResetPasswordManagerController(IUnitOfWork uow, IUserPasswordService userPasswordService)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));
            _userPasswordService = userPasswordService;
            _userPasswordService.NotNull(nameof(userPasswordService));
        }

        [HttpPost]
        [Route("reset-password")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ResetPassword([FromBody] UserIdDto userId, CancellationToken cancellationToken)
        {
            await _userPasswordService.ResetPasswordAsync(userId, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return Ok("وضعیت کاربر به -فعال- ویرایش شد");
        }
    }
}
