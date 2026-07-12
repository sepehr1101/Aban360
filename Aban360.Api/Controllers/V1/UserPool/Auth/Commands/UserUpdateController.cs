using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Constants;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Commands
{
    [Route("v1/user")]
    public class UserUpdateController : BaseController
    {
        private IUnitOfWork _uow;
        private IUserUpdateHandler _userUpdateHandler;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IOpLogCommandService _opLogCommandService;
        public UserUpdateController(
            IUnitOfWork uow,
            IUserUpdateHandler userUpdateHandler,
            IHttpContextAccessor contextAccessor,
            IOpLogCommandService opLogCommandService)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _userUpdateHandler = userUpdateHandler;
            _userUpdateHandler.NotNull(nameof(_userUpdateHandler));

            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(_contextAccessor));

            _opLogCommandService = opLogCommandService;
            _opLogCommandService.NotNull(nameof(_opLogCommandService));
        }
        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Trigger([FromBody] UserUpdateDto updateDto, CancellationToken cancellationToken)
        {
            UserPersonalGetDto beforEdit = await _userUpdateHandler.Handle(updateDto, CurrentUser, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            await InsertOpLog(updateDto, beforEdit);

            return Ok(MessageResources.UpdateUserSuccess);
        }
        private async Task InsertOpLog(UserUpdateDto updateDto, UserPersonalGetDto beforEditDto)
        {
            string opLogText = string.Format(OpLogLiterals.UserUpdate, updateDto.Id, beforEditDto.FullName, updateDto.FullName, beforEditDto.DisplayName, updateDto.DisplayName, beforEditDto.Mobile, updateDto.Mobile);
            await _opLogCommandService.Insert(opLogText, CurrentUser);
        }
    }
}