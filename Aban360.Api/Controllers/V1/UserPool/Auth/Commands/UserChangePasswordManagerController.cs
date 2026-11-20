using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Common.Constants;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Aban360.UserPool.Application.Features.Auth.Services;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Commands
{
    [Route("v1/user")]
    public class UserChangePasswordManagerController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserPasswordService _userPasswordService;
        public UserChangePasswordManagerController(IUnitOfWork uow, IUserPasswordService userPasswordService)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));
            _userPasswordService = userPasswordService;
            _userPasswordService.NotNull(nameof(_userPasswordService));
        }

        [HttpPost]
        [Route("change-password")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordInput changePasswordDto, CancellationToken cancellationToken)
        {
            await _userPasswordService.ChangePasswordAsync(changePasswordDto, CurrentUser.UserId, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return Ok(MessageLiterals.PasswordChangeSuccess);
        }
    }
}
