using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Delete.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Base;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Commands
{
    [Route("v1/user")]
    public class UserDeleteManagerController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserDeleteManagerHandler _userDeleteManagerHandler;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IOpLogCommandService _opLogCommandService;
        public UserDeleteManagerController(
            IUnitOfWork uow,
            IUserDeleteManagerHandler userDeleteManagerHandler,
            IHttpContextAccessor contextAccessor,
            IOpLogCommandService opLogCommandService)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _userDeleteManagerHandler = userDeleteManagerHandler;
            _userDeleteManagerHandler.NotNull(nameof(userDeleteManagerHandler));

            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(_contextAccessor));

            _opLogCommandService = opLogCommandService;
            _opLogCommandService.NotNull(nameof(_opLogCommandService));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] UserIdDto userId, CancellationToken cancellationToken)
        {
            await _userDeleteManagerHandler.Handle(userId, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            await InsertOpLog(userId);

            return Ok("کاربر با موفقیت حذف شد");
        }
        private async Task InsertOpLog(UserIdDto userId)
        {
            string opLogText = string.Format(OpLogLiterals.UserDelete, userId.Id);
            await _opLogCommandService.Insert(opLogText, CurrentUser);
        }
    }
}
