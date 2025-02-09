using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Delete.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Base;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Commands
{
    [Route("v1/user")]
    public class UserDeleteManagerController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserDeleteManagerHandler _userDeleteManagerHandler;
        public UserDeleteManagerController(
            IUnitOfWork uow,
            IUserDeleteManagerHandler userDeleteManagerHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _userDeleteManagerHandler = userDeleteManagerHandler;
            _userDeleteManagerHandler.NotNull(nameof(userDeleteManagerHandler));
        }

        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] UserIdDto userId, CancellationToken cancellationToken)
        {
            await _userDeleteManagerHandler.Handle(userId, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok("کاربر با موفقیت حذف شد");
        }
    }
}
