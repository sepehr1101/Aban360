using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Constants;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Commands
{
    [Route("v1/user")]
    public class UserUpdateController : BaseController
    {
        private IUnitOfWork _uow;
        private IUserUpdateHandler _userUpdateHandler;
        public UserUpdateController(
            IUnitOfWork uow,
            IUserUpdateHandler userUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _userUpdateHandler = userUpdateHandler;
            _userUpdateHandler.NotNull(nameof(_userUpdateHandler));
        }
        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Trigger([FromBody] UserUpdateDto userCreateDto ,CancellationToken cancellationToken)
        {
            await _userUpdateHandler.Handle(userCreateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(MessageResources.UpdateUserSuccess);
        }
    }
}